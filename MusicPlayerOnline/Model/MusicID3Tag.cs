using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model
{
    public class SongData
    {
        public string NameSong { get; set; }
        public string NameArtist { get; set; }
    }

  public  class MusicID3Tag

    {

        public byte[] TAGID = new byte[3];      //  3
        public byte[] Title = new byte[30];     //  30
        public byte[] Artist = new byte[30];    //  30 
        public byte[] Album = new byte[30];     //  30 
        public byte[] Year = new byte[4];       //  4 
        public byte[] Comment = new byte[30];   //  30 
        public byte[] Genre = new byte[1];      //  1

         
        public SongData DoParseFile(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                if (fs.Length >= 128)
                {
                    MusicID3Tag tag = new MusicID3Tag();
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(tag.TAGID, 0, tag.TAGID.Length);
                    fs.Read(tag.Title, 0, tag.Title.Length);
                    fs.Read(tag.Artist, 0, tag.Artist.Length);
                    fs.Read(tag.Album, 0, tag.Album.Length);
                    fs.Read(tag.Year, 0, tag.Year.Length);
                    fs.Read(tag.Comment, 0, tag.Comment.Length);
                    fs.Read(tag.Genre, 0, tag.Genre.Length);
                    string theTAGID = Encoding.Default.GetString(tag.TAGID);

                    if (theTAGID.Equals("TAG"))
                    {
                        string Title = Encoding.Default.GetString(tag.Title);
                        string Artist = Encoding.Default.GetString(tag.Artist);
                        string Album = Encoding.Default.GetString(tag.Album);
                        string Year = Encoding.Default.GetString(tag.Year);
                        string Comment = Encoding.Default.GetString(tag.Comment);
                        string Genre = Encoding.Default.GetString(tag.Genre);



                        SongData data = new SongData();

                        data.NameSong = Title;
                        data.NameArtist = Artist;
                      
                        return data;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            
        }
    }
}
