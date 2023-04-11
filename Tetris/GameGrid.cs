namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        // Méthode déterminant si on est à l'intérieur de la grid
        public bool IsInside(int r, int c)
        {
            return r >=  0 && r < Rows && c >= 0 && c < Columns;
        }

        // Méthode permettant de voir si une cellule est vide ou non
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0; // On vérifie que l'on est à l'intérieur de la grid et si la valeur vaut 0
        }

        // Méthode déterminant si une ligne est pleine
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++) // On parcourt les colonnes pour voir si la ligne a un 0
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true; // Si il n'y a aucun 0 dans la ligne, la ligne est pleine
        }

        // Méthode déterminant si une ligne est vide
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++) // On parcourt les colonnes pour voir si la ligne n'a pas de 0
            {
                if (grid[r,c] != 0)
                {
                    return false;
                }
            }
            return true; // Si il n'y a aucune valeur différente de 0, la ligne est vide
        }


        // Méthode permettant de clear une ligne pleine
        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        // Méthode permettant de descendre une ligne
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c]; // On attribut la valeur de la ligne qu'on veut descendre à la nouvelle ligne plus bas
                grid[r, c] = 0; // On met la ligne descendu à 0
            }
        }

        // Méthode permettant de clear toutes les lignes pleines de la grid
        public int ClearFullRows()
        {
            int cleared = 0; // Variable permettant de savoir de combien de lignes doit-on descendre les prochaines lignes

            for (int r = Rows-1; r >= 0; r--) // On commence par la dernière ligne, puis on remonte jusqu'en haut
            {
                if (IsRowFull(r)) // Si une ligne est pleine, on la clear et on incrémente "cleared" de 1
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0) // Si cleared n'est pas null, on déplace la ligne vers la bas par le nombre de lignes cleared
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
        }

    }
}
