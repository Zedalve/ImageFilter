
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Controls;


namespace ImageFilter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
        }

        private void btnLoadSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png files (*.png)|*.png|Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpg)|*.jpg";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                Bitmap sourceBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                picSource.BackgroundImage = sourceBitmap;

                OnCheckChangedEventHandler(sender, e);
            }
        }

       

        private void OnCheckChangedEventHandler(object sender, EventArgs e)
        {
            if (picSource.BackgroundImage != null)
            {
                if (lbFilter.SelectedIndex == 0)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.CopyAsGrayscale();
                }
                else if (lbFilter.SelectedIndex == 1)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.DrawAsGrayscale();
                }
                else if (lbFilter.SelectedIndex == 2)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.CopyWithTransparency();
                }
                else if (lbFilter.SelectedIndex == 3)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.DrawWithTransparency();
                }
                else if (lbFilter.SelectedIndex == 4)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.CopyAsNegative();
                }
                else if (lbFilter.SelectedIndex == 5)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.DrawAsNegative();
                }
                else if (lbFilter.SelectedIndex == 6)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.CopyAsSepiaTone();
                }
                else if (lbFilter.SelectedIndex == 7)
                {
                    picOutput.BackgroundImage = picSource.BackgroundImage.DrawAsSepiaTone();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
           // System.Console.WriteLine("Image: ");
           // System.Console.WriteLine(picOutput.Width);
           // System.Console.WriteLine(picOutput.Height);
            //System.Console.WriteLine(picOutput.CreateGraphics().ToString());
            //Bitmap bmp = new Bitmap(picOutput.Width, picOutput.Height, picOutput.CreateGraphics());
            //bmp.Save(@"C:\Temp\ImageOut.png");

            var fd = new SaveFileDialog();
            fd.Filter = "Bmp(*.BMP;)|*.BMP;| Jpg(*Jpg)|*.jpg";
            fd.AddExtension = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (Path.GetExtension(fd.FileName).ToUpper())
                {
                    case ".BMP":
                        picOutput.BackgroundImage.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".JPG":
                        picOutput.BackgroundImage.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".PNG":
                        picOutput.BackgroundImage.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }

        }

        

        //Removed buttons, now using list boxes as requested
       

       

        private void button7_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap image = new Bitmap(picOutput.BackgroundImage);
            picSource.BackgroundImage = image;
        }

        

        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            OnCheckChangedEventHandler(sender, e);
           
        }

        private void lbAdj_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (picSource.BackgroundImage != null)
            {
                if (lbAdj.SelectedIndex == 0)
                {

                    //Felt blur was too weak. Replaced with GaussianBlur which is stronger but requires more processing. 

                    System.Drawing.Bitmap image = new Bitmap(picSource.BackgroundImage);
                    // create filter
                    AForge.Imaging.Filters.GaussianBlur filter = new AForge.Imaging.Filters.GaussianBlur(4,11);
                    // apply filter
                    System.Drawing.Bitmap newImage = filter.Apply(image);

                    picOutput.BackgroundImage = newImage;

                    
                }
                else if (lbAdj.SelectedIndex == 1)
                {
                    System.Drawing.Bitmap image = new Bitmap(picSource.BackgroundImage);
                    // create filter
                    AForge.Imaging.Filters.Sharpen filter = new AForge.Imaging.Filters.Sharpen();
                    // apply filter
                    System.Drawing.Bitmap newImage = filter.Apply(image);

                    picOutput.BackgroundImage = newImage;
                }
                else if (lbAdj.SelectedIndex == 2)
                {
                    // enables up and down buttons, can be used for other adjustments
                    btnUp.Enabled = true;
                    btnDown.Enabled = true;

                    // Calls funtion in ExtBitmap.cs to manage brightness
                    System.Drawing.Bitmap newImage = picSource.BackgroundImage.ChangeBrightness();
                    picOutput.BackgroundImage = newImage;
                }
                else if (lbAdj.SelectedIndex == 3)
                {
                    System.Drawing.Bitmap image = new Bitmap(picSource.BackgroundImage);
                    // create filter
                    AForge.Imaging.Filters.Jitter filter = new AForge.Imaging.Filters.Jitter();
                    // apply filter
                    System.Drawing.Bitmap newImage = filter.Apply(image);

                    picOutput.BackgroundImage = newImage;
                }
                else if (lbAdj.SelectedIndex == 4)
                {
                    System.Drawing.Bitmap image = new Bitmap(picSource.BackgroundImage);
                    // create corner detector's instance
                    SusanCornersDetector scd = new SusanCornersDetector();

                    AForge.Imaging.Filters.CornersMarker filter = new AForge.Imaging.Filters.CornersMarker(scd, Color.Red);
                    // apply filter
                    System.Drawing.Bitmap newImage = filter.Apply(image);

                    picOutput.BackgroundImage = newImage;
                }
                

            }
        }
        /// <summary>
        /// Clears the selection of the Adjustments list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbFilter_Click(object sender, EventArgs e)
        {
            lbAdj.ClearSelected();
        }

        /// <summary>
        /// Clears the selection of the Filter list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void lbAdj_Click(object sender, EventArgs e)
        {
            lbFilter.ClearSelected();
        }

        /// <summary>
        /// Enables the brigtness button adjusters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAdj_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lbAdj.SelectedIndex != 2)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }

        /// <summary>
        /// Increases the default brightness of output image. Can be expanded for futher ultility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbAdj.SelectedIndex == 2)
            {
                System.Drawing.Bitmap newImage = picOutput.BackgroundImage.ChangeBrightnessUP();
                picOutput.BackgroundImage = newImage;
            }  
            
        }


        /// <summary>
        /// Lowers the default brightness of output image. Can be expanded for futher ultility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lbAdj.SelectedIndex == 2)
            {
                System.Drawing.Bitmap newImage = picOutput.BackgroundImage.ChangeBrightnessDOWN();
                picOutput.BackgroundImage = newImage;
            }
        }
    }
}
