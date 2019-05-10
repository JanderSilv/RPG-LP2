﻿using _3ReaisEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace _3ReaisEngine.UI
{
    using uwpUI = Windows.UI.Xaml.Controls;

    public delegate void Execute(object sender);

    public interface IUIEntidade
    {
        UIElement getElement();
        Vector2 getPosition();
        Vector2 getSize();
        string getName();
    }

    public interface IUIBackground
    {
        void setBackground(string path);
        void setBackground(string path, Stretch strech);
    }

    public class UButton: IUIEntidade, IUIBackground
    {
        uwpUI.Button element = new uwpUI.Button();
        TranslateTransform transform = new TranslateTransform();
        public string Nome;
        private Vector2 pos = new Vector2(), si = new Vector2(100,50);

        public object Content { get { return element.Content; } set { element.Content = value; } }
        public Vector2 position { get { return pos; } set { pos = value; transform.X = value.x; transform.Y = value.y; } }
        public Vector2 size { get { return si; } set { si = value; element.Width = value.x; element.Height = value.y; } }
        public Execute Action;
        ImageBrush brush = new ImageBrush();

        BitmapImage Normal;
        BitmapImage OnHover;
        BitmapImage OnClick;

        void Start()
        {
            element.Style = (Style)Application.Current.Resources["ButtonStyle"];
            VisualStateManager.GoToState(element, "Normal", false);
            element.HorizontalAlignment = HorizontalAlignment.Left;
            element.VerticalAlignment = VerticalAlignment.Top;
            element.RenderTransform = transform;
            element.PointerEntered += Element_PointerEntered;
            element.PointerExited += Element_PointerExited;
            element.Click += act;
         
           
        }

      
        public UButton(object Content, Execute Action = null)
        {
           
            element.Content = Content;        
            element.Width = 100;
            element.Height = 50;          
            transform.X = 0;
            transform.Y = 0;       
            this.Action = Action;
            Start();
        }

      

        public UButton(object Content,Vector2 position, Execute Action = null)
        {
            element.Content = Content;
            element.Width = 100;
            element.Height = 50;
            transform.X = position.x;
            transform.Y = position.y;
            pos = position;
            this.Action = Action;
            Start();
        }

        public UButton(object Content, Vector2 position,Vector2 size, Execute Action = null)
        {
            element.Content = Content;
            element.Width = size.x;
            element.Height = size.y;
            transform.X = position.x;
            transform.Y = position.y;
            pos = position;
            si = size;
            element.RenderTransform = transform;
            this.Action = Action;
            Start();
        }


        public void setBackground(string path)
        { 
            Normal = new BitmapImage(new Uri("ms-appx:/" + path));
            if (OnHover == null) OnHover = Normal;
            if (OnClick == null) OnClick = Normal;

            brush.Stretch = Stretch.UniformToFill;
            brush.ImageSource = Normal;
            element.Background = brush;
        }
        public void setBackground(string path, Stretch strech)
        {
            Normal = new BitmapImage(new Uri("ms-appx:/" + path));
            if (OnHover == null) OnHover = Normal;
            if (OnClick == null) OnClick = Normal;

            brush.ImageSource = Normal;
            element.Background = brush;
            brush.Stretch = strech;
        }

        public void setOnHover(string path)
        {
            OnHover = new BitmapImage(new Uri("ms-appx:/" + path));
        }
        public void setOnClick(string path)
        {
            OnClick = new BitmapImage(new Uri("ms-appx:/" + path));
        }

        private void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(OnHover!=null)
            brush.ImageSource = OnHover;
        }
        private void Element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            
            brush.ImageSource = Normal;
        }
     

        private void act(object sender, RoutedEventArgs e)
        {
  
            Action?.Invoke(sender);
        }

        public UIElement getElement()
        {
            return element;
        }

        public Vector2 getPosition()
        {
            return pos;
        }

        public Vector2 getSize()
        {
            return si;
        }

        public string getName()
        {
            return Nome;
        }

       
    }

   
}
