using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppSheetTest
{

    public partial class Form1 : Form
    {
        private string getListUrl = @"https://appsheettest1.azurewebsites.net/sample/list";
        private string getListTokenUrl = @"https://appsheettest1.azurewebsites.net/sample/list?token=";
        private string getDetailUrl = @"http://appsheettest1.azurewebsites.net/sample/detail/";
        ListViewColumnSorter lvwColumnSorter;

        public Form1()
        {
            InitializeComponent();
        }

        private List<dynamic> SortByAge(List<dynamic> peeps)
        {
            return peeps.OrderBy(p => p.age.Value).ToList();
        }

        private List<dynamic> SortByName(List<dynamic> peeps)
        {
            return peeps.OrderBy(p => p.name.Value).ToList();
        }

        private void BtnGetData_Click(object sender, EventArgs e)
        {
            List<string> listIds = new List<string>();
            List<dynamic> people = new List<dynamic>();

            if (GetIDs(ref listIds))
            {
                if (GetPeople(listIds, ref people))
                {
                    // Sort people by age
                    people = SortByAge(people);
                    // Add them to listview
                    AddPeopleToListView(people);
                }
            }
        }



        private void AddPeopleToListView(List<dynamic> peeps)
        {
            int totalPeeps = 0;

            // go through and add images first for listview
            for (int i = 0; i < peeps.Count; i++)
            {
                if (IsValidPhoneNumber(peeps[i].number.Value.ToString()))
                {
                    AddImage(peeps[i], totalPeeps);
                    totalPeeps++;

                    if (totalPeeps == 5)
                        break;
                }
            }

            listViewPeople.LargeImageList = peoplePictures;
            listViewPeople.SmallImageList = peoplePictures;

            // now add people
            totalPeeps = 0;
            for (int i = 0; i < peeps.Count; i++)
            {
                if (IsValidPhoneNumber(peeps[i].number.Value.ToString()))
                {
                    AddPerson(peeps[i], totalPeeps);
                    totalPeeps++;

                    if (totalPeeps == 5)
                        break;
                }
            }

            listViewPeople.Refresh();

            lvwColumnSorter.SortColumn = 1;
            lvwColumnSorter.Order = SortOrder.Ascending;
            this.listViewPeople.Sort();
        }

        private bool IsValidPhoneNumber(string number)
        {
            return Regex.Match(number, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").Success;
        }

        private void AddPerson(dynamic person, int index)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.ImageKey = index.ToString();
            lvi.Text = string.Empty;

            lvi.SubItems.Add(person.name.ToString());
            lvi.SubItems.Add(person.age.ToString());
            lvi.SubItems.Add(person.number.ToString());
            lvi.SubItems.Add(person.bio.ToString());

            listViewPeople.Items.Add(lvi);
        }

        private void AddImage(dynamic person, int index)
        {
            DownloadImage(index, person.photo.ToString());
        }

        private void DownloadImage(int index, string url)
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] data = wc.DownloadData(url);
                MemoryStream ms = new MemoryStream(data);
                Image pic = Image.FromStream(ms);

                Image resizedPic = ResizeImg(200, 200, pic);
                pic.Dispose();

                peoplePictures.Images.Add(index.ToString(), resizedPic);
                ms.Close();

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception downloading image url " + url + " : " + ex.Message);
            }

            // add blank image if not found
            Bitmap bmpBlank = new Bitmap(200, 200);
            peoplePictures.Images.Add(bmpBlank);
        }

        public Image ResizeImg(int newWidth, int newHeight, Image sourceImage)
        {
            int srcWidth = sourceImage.Width;
            int srcHeight = sourceImage.Height;
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(newWidth, newHeight);
            bitmap.MakeTransparent();

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            System.Drawing.Rectangle rectangleDestination = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
            graphics.DrawImage(sourceImage, rectangleDestination, 0, 0, srcWidth, srcHeight, System.Drawing.GraphicsUnit.Pixel);

            Stream destination = new MemoryStream();
            bitmap.Save(destination, ImageFormat.Png);
            destination.Position = 0;
            bitmap.Dispose();


            return Image.FromStream(destination);
        }


        /// <summary>
        ///  Get List of IDs of people
        /// </summary>
        /// <param name="listIds"></param>
        /// <returns></returns>
        private bool GetIDs(ref List<string> listIds)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    bool finishedGettingListIds = false;
                    string urlGetIDs = getListUrl;
                    int iterations = 0; // keep from going into an infinite loop - make sure we don't lose data though in production code

                    while (!finishedGettingListIds && iterations++ < 500)
                    {
                        var json = wc.DownloadString(urlGetIDs);
                        dynamic idList = JsonConvert.DeserializeObject(json);

                        for (int i = 0; i < idList.result.Count; i++)
                            listIds.Add(idList.result[i].Value.ToString());

                        if (idList.token == null)
                            break;

                        // set up url for next set of ids with token
                        urlGetIDs = getListTokenUrl + idList.token.Value.ToString();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception getting ID list: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Get details of each person via ID and add to list
        /// </summary>
        /// <param name="listIds"></param>
        /// <param name="people"></param>
        /// <returns></returns>
        private bool GetPeople(List<string> listIds, ref List<dynamic> people)
        {
            using (WebClient wc = new WebClient())
            {
                foreach (string id in listIds)
                {
                    string url = getDetailUrl + id;

                    try
                    {
                        var json = wc.DownloadString(url);
                        dynamic idDetail = JsonConvert.DeserializeObject(json);
                        people.Add(idDetail);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception getting details for URL " + url + ": " + ex.Message);
                    }
                }
            }

            // if we got some people then go ahead and work with what we have
            if (people.Count > 0)
                return true;

            return false;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lvwColumnSorter = new ListViewColumnSorter();
            listViewPeople.ListViewItemSorter = lvwColumnSorter;
            listViewPeople.View = View.Details;
            peoplePictures.ImageSize = new Size(200, 200);

        }

        private void ListViewPeople_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listViewPeople.Sort();
        }

        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }
        }
    }
}
