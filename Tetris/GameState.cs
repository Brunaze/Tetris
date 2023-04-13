using System;

namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;
        public Block CurrentBlock
        {
            get => currentBlock; 
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public int Cleared { get; private set; }
        public int Level { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState() // Initialisation du jeu
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
            Level = 1;
        }

        // Méthode permettant de hold un block
        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        // Méthode déterminant si le block actuel est dans une position autorisé ou non
        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions()) // Parcourt les 4 positions du block actuel pour voir si il est hors de la grid ou sur une case pas vide
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        // Méthode pour tourner le block dans le sens horaire SEULEMENT SI c'est possible
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits()) // On tourne et si l'objet ne fit pas, on le retourne dans le sens inverse
            {
                CurrentBlock.RotateCCW();
            }
        }

        // Méthode pour tourner le block dans le sens anti horaire SEULEMENT SI c'est possible
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        // Méthode pour bouger le block vers la gauche
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits()) // Si impossible, on revient 
            {
                CurrentBlock.Move(0, 1);
            }
        }

        // Méthode pour bouger le block vers la droite
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits()) // Si impossible, on revient
            {
                CurrentBlock.Move(0, -1);
            }
        }

        // Méthode déterminant si le jeu est terminé
        public bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1)); // Si une des deux lignes cachées du haut ne sont pas vide, c'est perdu
        }

        // Méthode permettant de calculer le score
        private int Scoring(int cleared, int level)
        {
            int score = 0;

            switch(cleared)
            {
                case 1:
                    score = 40 * level;
                    return score;
                case 2:
                    score = 100 * level;
                    return score;
                case 3:
                    score = 300 * level;
                    return score;
                case 4:
                    score = 1200 * level;
                    return score;
                default: 
                    return score;
            }

        }

        // Méthode appelé lorsque le block ne peut pas descendre plus (placer le block)
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions()) // Place le numéro d'id du block dans sa position actuelle
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            int clear = 0;

            clear = GameGrid.ClearFullRows(); // Ensuite, on clear toutes les possibles lignes pleines
            Score += Scoring(clear, Level);
            Cleared += clear;
            Level = (Cleared / 10) + 1;

            if (IsGameOver()) // On vérifie si le jeu est perdu
            {
                GameOver = true;
            }
            else // Sinon, on update le block
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        // Méthode pour se déplacer vers le bas
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits()) // Si impossible, on revient et on place le block avec la méthode juste au dessus
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        // Méthode pour voir la distance du drop pour une tile
        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        // Méthode pour voir la distance du drop du block
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions()) // Pour chaque tile du block actuel, on récupère le drop et on prend le plus petit
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        // Méthode permettant de drop le block
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

    }
}
