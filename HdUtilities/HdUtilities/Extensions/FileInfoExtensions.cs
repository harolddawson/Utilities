using System;
using System.IO;
using HdUtilities.Enum;

namespace HdUtilities.Extensions
{
    public static class FileInfoExtensions
    {
        public static FileTypeEnum GetFileType(this string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ArgumentNullException(nameof(fullPath));
            }
            switch (Path.GetExtension(fullPath).ToLower())
            {
                case ".cs":
                    return FileTypeEnum.CSharp;
                case ".cpp":
                case ".h":
                    return FileTypeEnum.CPlusPlus;
                case ".java":
                    return FileTypeEnum.Java;
                case ".db":
                    return FileTypeEnum.SQLiteDatabase;
                case ".json":
                    return FileTypeEnum.Json;
                case ".xml":
                    return FileTypeEnum.Xml;
                case ".ds_store":
                case ".ini":
                    return FileTypeEnum.FileSystemReference;
                case ".txt":
                case ".log":
                    return FileTypeEnum.Text;
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".bmp":
                    return FileTypeEnum.Picture;
                case ".gif":
                    return FileTypeEnum.Gif;
                case ".pdf":
                    return FileTypeEnum.Pdf;
                case ".mpg":
                case ".mpeg":
                case ".mp4":
                case ".wmv":
                case ".avi":
                case ".ts":
                case ".mov":
                case ".flv":
                case ".swf":
                case ".asf":
                case ".3g2":
                case ".m4v":
                case ".divx":
                case ".mkv":
                case ".3gp":
                case ".ram":
                case ".mpa":
                case ".asx":
                case ".qt":
                    //Real Player files
                case ".rm":
                case ".ra":
                case ".ivr":
                    return FileTypeEnum.Movie;
                    
                case ".rar":
                case ".zip":
                case ".7z":
                case ".tar":
                case ".tgz":
                    return FileTypeEnum.Zip;

                case ".wav":
                case ".mp3":
                case ".ogg":
                    return FileTypeEnum.Audio;
                case ".htm":
                case ".html":
                    return FileTypeEnum.WebHtml;

                case ".crdownload":
                    return FileTypeEnum.ChromeDownload;
                case ".bat":
                    return FileTypeEnum.Batch;

                case ".csv":
                    return FileTypeEnum.CommaDelimited;
                case ".xls":
                case ".xlsx":
                    return FileTypeEnum.MsExcel;
                case ".doc":
                case ".docx":
                    return FileTypeEnum.MsWord;

                case ".ps":
                
                case ".url":
                case "":
                case ".lnk":
                case ".meta":
                case ".vob":
                
                case ".torrent":
                
                case ".ithmb":
                case ".thm":
                case ".sfk":
                case ".exe":
                case ".thumb":
                case ".mswmm":
                case ".info":

                default:
                    return FileTypeEnum.Other;

            }
        }
        public static bool IsDuplicateOf(this FileInfo fi1, FileInfo fi2)
        {
            if (fi1 == null && fi2 == null)
            {
                return true;
            }

            if (fi1 == null ^ fi2 == null)
            {
                return false;
            }

            if (fi1.Equals(fi2))
            {
                return true;
            }

            if (!string.Equals(fi1.Name, fi2.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (fi1.Length == fi2.Length)
            {
                return true;
            }

            return false;
        }
    }
}