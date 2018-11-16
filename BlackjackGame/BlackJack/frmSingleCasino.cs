using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//William Tran
//Nov 6, 2018
//Singleplayer Casino Blackjack
//The player play a game of blackjack against the dealer in the casino style

namespace BlackJack
{
    public partial class frmSingleCasino : Form
    {
        enum BetState //controls if bet is increased or decreased
        {
            Increase,
            Decrease,
            None
        }

        private Deck mDeck; //whole deck of cards
        private Hand mPlayer; //cards in player's hand
        private Hand mDealer; //cards in dealer's hand
        private int mCash; //keeps track of player's money
        private int mStartingCash; //how much players start with in each game
        private int mBet; //how much the player has bet
        private BetState PlayerBetState = BetState.None;


        public frmSingleCasino(int StartingCash)
        {
            InitializeComponent();
            mStartingCash = StartingCash;
            NewGame();
        }

        private void mnuGameNewGame_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void mnuGameHelp_Click(object sender, EventArgs e)
        {
            //show message box with all the rules
            MessageBox.Show("Rules:\n" +
                "Taken from https://wizardofodds.com/games/blackjack/basics/ \n" +
                "The object of the game is to beat the dealer.\n" +
                "Aces may be counted as 1 or 11 points, 2 to 9 according to pip value, and tens and face cards count as ten points.\n" +
                "The value of a hand is the sum of the point values of the individual cards. Except, a \"blackjack\" is the highest hand, consisting of an ace and any 10-point card, and it outranks all other 21-point hands.\n" +
                "After the player has bet, the dealer will give two cards to each player and two cards to himself. One of the dealer cards is dealt face up.\n" +
                "If the dealer does have a blackjack, then the player loses, unless the player also has a blackjack, which will result in a tie.\n" +
                "\n" +
                "Possible player choices:\n" +
                "Stand: Player stands pat with his cards.\n" +
                "Hit: Player draws another card (and more if he wishes). If this card causes the player's total points to exceed 21 (known as \"busting\") then he loses.\n" +
                "Double: Player doubles his bet and gets one, and only one, more card.\n" +
                "\n" +
                "After the player turn, the dealer will add card to his own hand until he gets at least 17 points.\n" +
                "If the dealer busts, the player wins.\n" +
                "If the dealer does not bust, then the higher point total between the player and dealer will win.\n" +
                "Winning wagers pay even money, except a winning player blackjack usually pays 3 to 2\n" +
                "\n" +
                "Shortcuts:\n" +
                "'A' = Deal\n" +
                "'D' = Hold\n" +
                "'Q' = Double\n" +
                "'Space' = Confirm Bet");
        }

        private void mnuGameExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); //end application  
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            Deal();
        }

        private void Deal()
        {
            //add a card to the players hand and redraw
            mPlayer.AddCard(mDeck.Deal());
            Invalidate(); //redraw

            //if player is bust, calculate money won or lost
            if (mPlayer.getScore() > 21) EndRound();
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            Hold();
        }

        private void Hold()
        {
            mDealer.getCard(1).FaceDown = false; //flip up dealers second card and redraw
            while (mDealer.getScore() < 17) mDealer.AddCard(mDeck.Deal()); //keep giving dealer cards until he is at least at 17 points
            this.Invalidate();
            EndRound(); //calculate money won or lost
        }

        protected override void OnPaint(PaintEventArgs e) //override OnPaint event
        {
            base.OnPaint(e);
            //draw hand
            if (mPlayer != null) mPlayer.DrawHand(e.Graphics, 24, 207);
            if (mDealer != null) mDealer.DrawHand(e.Graphics, 208, 60);
        }

        private void StartNewRound() //resets deck and hand, shuffles, adds 2 cards to hand
        {
            //shuffle deck and give player 2 cards to start
            mDeck.Shuffle();
            mPlayer.AddCard(mDeck.Deal());
            mPlayer.AddCard(mDeck.Deal());

            //give dealer 2 cards, one up and one down
            mDealer.AddCard(mDeck.Deal());
            mDealer.AddCard(mDeck.DealFaceDown());

            //repaint form
            this.Invalidate();

            //if player or dealer has blackjack, calculate money won
            if (mPlayer.getScore() == 21 || mDealer.getScore() == 21)
            {
                mDealer.getCard(1).FaceDown = false; //flip up dealers second card and redraw
                this.Invalidate();
                EndRound();
            }
        }

        private void EndRound()
        {
            int PlayerScore = mPlayer.getScore(); //value of all cards in player's hand
            int DealerScore = mDealer.getScore(); //value of all cards in dealer's hand

            //if statement for each round end condition
            //each condition changes the players money and shows what the player got
            if (PlayerScore > 21)
            {
                MessageBox.Show("Bust.\nLose $" + mBet + ".");
                mCash -= mBet;
            }
            else if (DealerScore > 21)
            {
                MessageBox.Show("Dealer bust.\nWin $" + mBet + ".");
                mCash += mBet;
            }
            else if (PlayerScore == 21 && DealerScore == 21)
            {
                if (mPlayer.GetNumOfCards == 2 && mDealer.GetNumOfCards == 2)
                {
                    MessageBox.Show("Both player and dealer have Blackjack.\nBet of $" + mBet + " returned.");
                }
                else if (mPlayer.GetNumOfCards == 2)
                {
                    MessageBox.Show("Player's Blackjack beats dealer's 21.\nWin $" + mBet + ".");
                    mCash += mBet;
                }
                else if (mDealer.GetNumOfCards == 2)
                {
                    MessageBox.Show("Dealer's Blackjack beats players's 21.\nLose $" + mBet + ".");
                    mCash -= mBet;
                }
                else
                {
                    MessageBox.Show("Both player and dealer have 21 points.\nBet of $" + mBet + " returned.");
                }
            }
            else if (PlayerScore > DealerScore)
            {
                MessageBox.Show("Player has higher score than dealer.\nWin bet of $" + mBet + ".");
                mCash += mBet;
            }
            else if (PlayerScore == DealerScore)
            {
                MessageBox.Show("Player and dealer have same score.\nBet of $" + mBet + " returned.");
            }
            else //only other possibility is if PlayerScore < DealerScore
            {
                MessageBox.Show("Player has lower score than dealer.\nLost bet of $" + mBet + ".");
                mCash -= mBet;
            }

            //change text of bank
            lblCashAmount.Text = "Player Bank: $" + mCash;

            //check for if money is left
            if (mCash <= 0)
            {
                //player loses and turn off buttons
                MessageBox.Show("You lose.\nNo money is left.");
                btnDeal.Enabled = false;
                btnHold.Enabled = false;
            }
            else
            {
                //start next round
                if (mBet > mCash) //change bet if it is higher than cash
                {
                    mBet = mCash;
                    lblBetAmount.Text = "Bet: $" + mBet;
                }
                StartBettingStage();
            }
        }

        private void StartBettingStage()
        {
            //initialize deck and hand
            mDeck = new Deck();
            mPlayer = new Hand();
            mDealer = new Hand();
            //redraw
            this.Invalidate();

            btnDeal.Enabled = false;
            btnHold.Enabled = false;
            btnDouble.Enabled = false;
            btnConfirmBet.Enabled = true;
            tmrBetting.Enabled = true;

        }

        private void EndBettingStage()
        {
            btnDeal.Enabled = true;
            btnHold.Enabled = true;
            btnConfirmBet.Enabled = false;
            tmrBetting.Enabled = false;
            if ((mBet * 2) <= mCash) btnDouble.Enabled = true;
            StartNewRound();
        }

        private void NewGame()
        {
            //reset and show dollars left
            mCash = mStartingCash;
            lblCashAmount.Text = "Player Bank: $" + mCash;
            mBet = 1;

            //reset buttons
            btnDeal.Enabled = false;
            btnHold.Enabled = false;

            StartBettingStage(); //resets deck and hand, shuffles, adds 2 cards to hand
        }

        private void tmrBetting_Tick(object sender, EventArgs e) //ticks during betting to see if player want to increase/decrease bet
        {
            //bets must be able to be increased/decreased to be changed
            if((PlayerBetState == BetState.Increase && mBet < mCash )|| (PlayerBetState == BetState.Decrease && mBet > 1))
            {
                if (PlayerBetState == BetState.Increase) mBet++;
                else mBet--;
                lblBetAmount.Text = "Bet: $" + mBet;
            }
            
        }

        private void frmSingleCasino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && btnDeal.Enabled == true)
            {
                Deal();
            }
            else if (e.KeyCode == Keys.D && btnHold.Enabled == true)
            {
                Hold();
            }
            else if (e.KeyCode == Keys.Q && btnDouble.Enabled == true)
            {
                Double();
            }
            else if (e.KeyCode == Keys.W && tmrBetting.Enabled == true)
            {
                PlayerBetState = BetState.Increase;
            }
            else if (e.KeyCode == Keys.S && tmrBetting.Enabled == true)
            {
                PlayerBetState = BetState.Decrease;
            }
            else if (e.KeyCode == Keys.Space && btnConfirmBet.Enabled == true)
            {
                EndBettingStage();
            }
        }

        private void frmSingleCasino_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && tmrBetting.Enabled == true)
            {
                PlayerBetState = BetState.None;
            }
            else if (e.KeyCode == Keys.S && tmrBetting.Enabled == true)
            {
                PlayerBetState = BetState.None;
            }
        }

        private void btnConfirmBet_Click(object sender, EventArgs e)
        {
            EndBettingStage();
        }

        private void btnDouble_Click(object sender, EventArgs e)
        {
            Double();
        }

        private void Double()
        {
            mBet *= 2;
            lblBetAmount.Text = "Bet: $" + mBet;

            //add a card to the players hand and redraw
            mPlayer.AddCard(mDeck.Deal());
            Invalidate(); //redraw

            //if player is bust, calculate money won or lost
            if (mPlayer.getScore() > 21) EndRound();
            else Hold();

        }
    }
}
