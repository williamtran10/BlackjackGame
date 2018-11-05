using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//William Tran
//Oct 23, 2018
//Blackjack Game
//This program lets the player play a game of blackjack against the dealer

namespace BlackJack
{
    public partial class frmMain : Form
    {
        private Deck mDeck; //whole deck of cards
        private Hand mPlayer1; //cards in player's hand
        private int mDollars; //keeps track of player's money

        public frmMain()
        {
            InitializeComponent();
        }

        private void fileNew_Click(object sender, EventArgs e)
        {
            //reset and show dollars left
            mDollars = 100;
            lblDollarValue.Text = "Player Bank: $100";

            //reset buttons
            btnDeal.Enabled = true;
            btnHold.Enabled = true;

            StartNewRound(); //resets deck and hand, shuffles, adds 2 cards to hand
        }

        private void fileExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); //end application          
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            //add a card to the players hand and redraw
            mPlayer1.AddCard(mDeck.Deal());
            this.Invalidate(); //redraw

            //if player is bust, calculate money won or lost
            if (mPlayer1.getScore() > 21)
            {
                EndRound();
            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            EndRound(); //calculate money won or lost
        }

        protected override void OnPaint(PaintEventArgs e) //override OnPaint event
        {
            base.OnPaint(e);

            //draw hand
            if (mPlayer1 != null)
            {
                mPlayer1.DrawHand(e.Graphics);
            }
        }

        private void StartNewRound() //resets deck and hand, shuffles, adds 2 cards to hand
        {
            //initialize deck and hand
            mDeck = new Deck();
            mPlayer1 = new Hand();

            //shuffle deck and give player 2 cards to start
            mDeck.Shuffle();
            mPlayer1.AddCard(mDeck.Deal());
            mPlayer1.AddCard(mDeck.Deal());

            //repaint form
            this.Invalidate();

            //if player is already bust, calculate money won or lost
            if (mPlayer1.getScore() > 21)
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            int Score = mPlayer1.getScore(); //value of all cards in hand

            //if statement for each round end condition
            //each condition changes the players money and shows what the player got
            if (Score > 21)
            {
                mDollars -= 10;
                MessageBox.Show("Bust.\nLose $10.");
            }
            else if (Score == 21)
            {
                mDollars += 20;
                MessageBox.Show("Black Jack.\nWin $20.");
            }
            else if (Score >= 18)
            {
                mDollars += 10;
                MessageBox.Show("Beat the dealer.\nWin $10.");
            }
            else
            {
                mDollars -= 5;
                MessageBox.Show("Lower than dealer.\nLose $5.");
            }

            //change text of bank
            lblDollarValue.Text = "Player Bank: $" + mDollars;

            //check for if money is left
            if (mDollars <= 0)
            {
                //player loses and turn off buttons
                MessageBox.Show("You lose.\nNo money is left.");
                btnDeal.Enabled = false;
                btnHold.Enabled = false;
            }
            else
            {
                //start next round
                StartNewRound();
            }
        }
    }
}
