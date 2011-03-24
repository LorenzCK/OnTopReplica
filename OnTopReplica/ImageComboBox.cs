using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OnTopReplica {
    class ImageComboBox : ComboBox {

        public ImageComboBox() {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs ea) {
            ea.DrawBackground();
            ea.DrawFocusRectangle();

            if (ea.Index == -1)
                return;

            Rectangle bounds = ea.Bounds;
            var foreBrush = new SolidBrush(ea.ForeColor);
            int textLeftBound = (IconList == null) ? bounds.Left : bounds.Left + IconList.ImageSize.Width;

            var drawObject = Items[ea.Index];
            if (drawObject is ImageComboBoxItem) {
                var drawItem = (ImageComboBoxItem)drawObject;

                if (drawItem.ImageListIndex != -1 && IconList != null) {
                    //ea.Graphics.FillRectangle(Brushes.Gray, bounds.Left, bounds.Top, IconList.ImageSize.Width, IconList.ImageSize.Height);
                    ea.Graphics.DrawImage(IconList.Images[drawItem.ImageListIndex], bounds.Left, bounds.Top);
                }

                ea.Graphics.DrawString(drawItem.Text, ea.Font, foreBrush, textLeftBound, bounds.Top);
            }
            else {
                ea.Graphics.DrawString(drawObject.ToString(), ea.Font, foreBrush, textLeftBound, bounds.Top);
            }

            base.OnDrawItem(ea);
        }

        public ImageList IconList { get; set; }

    }

    class ImageComboBoxItem {

        public ImageComboBoxItem() {
            Text = "";
            ImageListIndex = -1;
        }

        public ImageComboBoxItem(string text) {
            if (text == null)
                throw new ArgumentNullException();

            Text = text;
            ImageListIndex = -1;
        }

        public ImageComboBoxItem(string text, int imageListIndex) {
            if (text == null)
                throw new ArgumentNullException();

            Text = text;
            ImageListIndex = imageListIndex;
        }

        public string Text { get; private set; }

        public int ImageListIndex { get; private set; }

        public object Tag { get; set; }

    }
}
