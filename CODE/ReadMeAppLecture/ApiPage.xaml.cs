using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Xml;
using System.Net.Http;
using static Android.Icu.Text.CaseMap;
using static Android.Provider.MediaStore;

namespace ReadMeAppLecture;

public partial class ApiPage : ContentPage
{
	HttpClient client = new();
	bool useXml = false;

	public ApiPage()
	{
		InitializeComponent();
	}

	private async void Load_Button(object sender, EventArgs e)
	{
		try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://10.0.2.2:3000/epub/1");
            HttpResponseMessage response = await client.SendAsync(request);


            //Call API
            //var response = await client.GetAsync("http://10.0.2.2:3000/epub/1");
           
			if (response.IsSuccessStatusCode)
            { 			

				var content = response.Content;

				//Open EPUB in Zip
				ZipArchive archive = new ZipArchive(content.ReadAsStream());
                var coverEntry = archive.GetEntry("OEBPS/Images/cover.png");
                var coverStream = coverEntry.Open();

                //Attach cover to UI
                cover.Source = ImageSource.FromStream(() => coverStream);

                //Load CONTENT (meta data)
                var bookTitle = "not found";
                var contentString = new StreamReader(archive.GetEntry("OEBPS/content.opf").Open()).ReadToEnd();

                if (useXml)
                {
                    #region XML version
                    //load meta-data from xml
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(contentString);

                    // Retrieve the title element
                    XmlNode titleNode = xmlDoc.SelectSingleNode("//dc:title", GetNamespaceManager(xmlDoc));

                    bookTitle = titleNode != null ? titleNode.InnerText : "not found with xml";

                    #endregion

                }
                else
                {
                    #region plain text version
                    int start = contentString.IndexOf("<dc:title>") + 10;
                    int end = contentString.IndexOf("</dc:title>");

                    bookTitle = (start != -1 && end != -1) ? contentString.Substring(start, end - start) : "Title node not found.";
                    #endregion
                }
                title.Text = bookTitle;
                //await Navigation.PushAsync(new ReadPage());



            }
            else
            {
                throw new Exception($"Bad status : {response.StatusCode}, {response.Headers},{response.Content}");
            }
        }
		catch (Exception ex)
		{
            await DisplayAlert(ex.Message, ex.StackTrace, "ok");
            Debug.WriteLine(ex.Message + "ERROOOOOOR");

        }
    }
    private async void Cover_Clicked(object sender, System.EventArgs e)
    {
        // Aquí puedes navegar a la página de lectura del libro
        await Navigation.PushAsync(new ReadPage());
    }

    private static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
    {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
        nsManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
        return nsManager;
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        useXml = e.Value;
    }

}