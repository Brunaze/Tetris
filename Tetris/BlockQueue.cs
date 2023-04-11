using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random();

        public Block NextBlock {  get; private set; }

        public BlockQueue() // On initialise la queue avec un block aléatoire
        {
            NextBlock = RandomBlock();
        }

        // Méthode permettant de retourner un block aléatoire
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        // Méthode renvoie le prochain block et update les propriétés
        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do // On ne veut pas avoir le même block deux fois d'affilée
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
