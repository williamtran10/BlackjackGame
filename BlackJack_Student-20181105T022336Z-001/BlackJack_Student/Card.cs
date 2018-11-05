using System;
using System.Drawing;
using System.Drawing.Drawing2D;

//William Tran
//Oct 23, 2018
//Card Class
//Holds each card's name and value

namespace BlackJack
{
    class Card
    {
        //*************************************************************
        //Private Fields
        //*************************************************************

        private int mValue;  //face value of the card 2-11
        private string mCardName;  //card name (in resource file) for the card to draw it

        //*************************************************************
        //Constructors
        //*************************************************************

        public Card(int Value, string CardName)
        {
            //use value and name given
            this.mValue = Value;
            this.mCardName = CardName;
        }

        //*************************************************************
        //Properties (accessor and mutators)
        //*************************************************************
        
        public int CardValue //property to get access to the card value
        {
            get { return mValue; }
        }

        //*************************************************************
        //Methods
        //*************************************************************

        public void DrawCard(Graphics g, int x, int y)
        {
            //draws this card at the location x and y 
            Bitmap CardToDraw = (Bitmap)Resource1.ResourceManager.GetObject(mCardName);
            g.DrawImage(CardToDraw, x, y);
        }
    }
}
