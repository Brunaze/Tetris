using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; } // Position du block actuel
        protected abstract Position StartOffset { get; } // Position d'apparition du block
        public abstract int Id { get; } // Id pour distinguer les blocks

        private int rotationState; // Rotation du block entre 0 et 3
        private Position offset; // Ecart avec le bord de gauche (row) et le bord du haut (column)

        public Block() // Iniialisé le offset d'un nouveau block à l'offset de départ
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        // Méthode retournant les positions occupés par le block en prenant en compte la rotation et offset actuel
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // Méthode permettant de tourner le block de 90° dans le sens horaire
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // Méthode permettant de tourner le block de 90° dans le sens anti horaire
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        // Méthode permettant de bouger le block d'un certain nombre de lignes et de colonnes
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        // Méthode permettant de reset la rotation et la position 
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

    }
}
