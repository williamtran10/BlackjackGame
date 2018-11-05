using System;
using System.Drawing;
using System.Drawing.Drawing2D;

//William Tran
//Oct 23, 2018
//Hand Class
//Holds the cards that are currently in the player's hand

namespace BlackJack
{
    class Hand
    {
        //*************************************************************
        //Private Fields
        //*************************************************************

        private Card[] mHand; //players hand
        private int mCards; //number of cards in hand

        //*************************************************************
        //Constructor
        //*************************************************************

        public Hand() //make empty hand with 0 cards
        {
            mHand = new Card[52];
            this.mCards = 0;
        }

        //*************************************************************
        //Properties
        //*************************************************************

        //*************************************************************
        //Methods
        //*************************************************************

        public void AddCard(Card Card)
        {
            //assign this card to hand and increment card counter
            this.mHand[this.mCards] = Card;
            this.mCards++;
        }

        public void DrawHand(Graphics g)
        {
            int X = 0; //x value that the cards start at

            //loop through each card in the hand 
            for (int i = 0; i < this.mCards; i++)
            {
                //draw card then add 80 to x so that the next card will be drawn beside it
                this.mHand[i].DrawCard(g, X, 100);
                X += 80;
            }
        }


        public int getScore()
        {
            //adds up and returns total value of the cards
            int Score = 0;
            for (int i = 0; i < this.mCards; i++) //loop through each card 
            {
                Score += mHand[i].CardValue;
            }
            return Score;
        }

    }
}
