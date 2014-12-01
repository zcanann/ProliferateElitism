using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace River
{
    //Used to store information when swapping items
    static class SwapHelper
    {
        private enum ConnectionType
        {
            None,
            Equipment,
            EnemyInventory,
            Shop,
            Storage,
            Enchanting,
        }

        private static ConnectionType Connection = ConnectionType.None;

        public static Texture2D IconSelector;
        public static Texture2D IconSelected;

        private static StandardInventory ConnectedInventory;
        private static StandardInventory NormalInventory;

        private static byte PrioritizedIndex = 0;
        private static byte NextIndex = 0;
        private static StandardInventory[] SwapCallers = new StandardInventory[2];
        private static int[] StoredIndexes = new int[2];

        public static void LoadContent(ContentManager Content)
        {
            IconSelector = Content.Load<Texture2D>(@"Textures\Inventory\IconSelector");
            IconSelected = Content.Load<Texture2D>(@"Textures\Inventory\IconSelected");
        }

        public static void Connect(StandardInventory Caller)
        {
            NormalInventory = Caller;
        }
        public static void Connect(Equipment Caller)
        {
            ConnectedInventory = Caller;
            Connection = ConnectionType.Equipment;
        }
        public static void Connect(LootInventory Caller)
        {
            ConnectedInventory = Caller;
            Connection = ConnectionType.EnemyInventory;
        }
        public static void Connect(ShopInventory Caller)
        {
            ConnectedInventory = Caller;
            Connection = ConnectionType.Shop;
        }
        public static void Connect(StorageInventory Caller)
        {
            ConnectedInventory = Caller;
            Connection = ConnectionType.Storage;
        }
        public static void Connect(EnchantingInventory Caller)
        {
            ConnectedInventory = Caller;
            Connection = ConnectionType.Enchanting;
        }

        public static void Push(StandardInventory Caller, int Index)
        {
            //Store inventory and index
            SwapCallers[NextIndex] = Caller;
            StoredIndexes[NextIndex] = Index;

            //Check for empty first selection
            if (SwapCallers[0].Items[StoredIndexes[0]] == Item.None)
            {
                Reset();
                return;
            }

            //If the caller is the inventory we are connected to, then we need to save that index
            if (Caller == ConnectedInventory)
                PrioritizedIndex = NextIndex;

            //Both have been filled!
            if (NextIndex == 1)
                DoSwap();

            NextIndex ^= 1;
        }

        public static bool HasSelection()
        {
            if (NextIndex == 1)
                return true;

            return false;
        }

        public static StandardInventory GetSelectionInventory()
        {
            return SwapCallers[0];
        }


        public static void Reset()
        {
            NextIndex = 0;
        }

        public delegate void SwapDisconnect();
        private static SwapDisconnect DisconnectFunction;

        public static void SetDisconnectCallBack(SwapDisconnect _DisconnectFunction)
        {
            DisconnectFunction = _DisconnectFunction;
        }

        public static void Disconnect()
        {
            if (DisconnectFunction != null)
            {
                //Run the disconnect function
                DisconnectFunction();
                DisconnectFunction = null;
            }

            //If this class is used right this is redundant, but just in case.
            Reset();
        }

        private static void DoSwap()
        {
            //1) Check to see if going to another menu
            bool CrossMenu = false;
            int ConnectedSlot = -1;
            int NormalSlot = -1;

            //a) Test for double click swap
            if (SwapCallers[0] == SwapCallers[1] &&
                StoredIndexes[0] == StoredIndexes[1])
                CrossMenu = true;

            //b) Test for standard swap
            else if (SwapCallers[0] != SwapCallers[1])
                CrossMenu = true;

            switch (Connection)
            {
                case ConnectionType.Shop:
                    if (CrossMenu)
                    {
                        //Check if double click BUY ITEM
                        if (SwapCallers[PrioritizedIndex ^ 1] == ConnectedInventory)
                        {
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];

                            //Try to find an empty inventory slot
                            for (int ecx = 0; ecx < NormalInventory.Items.Length; ecx++)
                            {
                                if (NormalInventory.Items[ecx] == Item.None)
                                {
                                    NormalSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (NormalSlot == -1)
                                return;
                        }
                        //Check if double click SELL ITEM
                        else if (SwapCallers[PrioritizedIndex] != ConnectedInventory)
                        {
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];

                            //Try to find an empty enemy slot
                            for (int ecx = 0; ecx < ConnectedInventory.Items.Length; ecx++)
                            {
                                if (ConnectedInventory.Items[ecx] == Item.None)
                                {
                                    ConnectedSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (ConnectedSlot == -1)
                                return;
                        }

                        else //Standard swap
                        {
                            //Grab slot
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];
                        }

                        //Apply swap
                        ShopInventory.SwapItems(ConnectedInventory, ConnectedSlot, NormalInventory, NormalSlot);
                    }
                    else
                    {
                        //Just an Inventory <<>> Inventory or Shop <<>> Shop
                        ShopInventory.SwapItems(SwapCallers[0], StoredIndexes[0], SwapCallers[1], StoredIndexes[1]);
                    }
                    break;

                case ConnectionType.Equipment:

                    if (CrossMenu)
                    {
                        //Check if double click UNEQUIP
                        if (SwapCallers[PrioritizedIndex ^ 1] == ConnectedInventory)
                        {
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];

                            //Try to find an empty inventory slot
                            for (int ecx = 0; ecx < NormalInventory.Items.Length; ecx++)
                            {
                                if (NormalInventory.Items[ecx] == Item.None)
                                {
                                    NormalSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (NormalSlot == -1)
                                return;
                        }
                        //Check if double click EQUIP
                        else if (SwapCallers[PrioritizedIndex] != ConnectedInventory)
                        {
                            ConnectedSlot = (int)NormalInventory.Items[StoredIndexes[PrioritizedIndex ^ 1]].Slot;

                            if (ConnectedSlot == (int)Item.SlotType.None)
                                return;
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];
                        }

                        else //Standard swap
                        {
                            //Grab slot
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];

                            if (NormalInventory.Items[NormalSlot] != Item.None)
                                if ((int)NormalInventory.Items[NormalSlot].Slot != ConnectedSlot)
                                    return;
                        }

                        //Apply swap
                        Equipment.SwapItems(ConnectedInventory, ConnectedSlot, NormalInventory, NormalSlot);

                    }
                    else
                    {
                        //Check for Equipment <<>> Equipment swap (not allowed)
                        if (SwapCallers[PrioritizedIndex] == ConnectedInventory)
                            break;

                        //Just an Inventory <<>> Inventory
                        StandardInventory.SwapItems(NormalInventory, StoredIndexes[0], NormalInventory, StoredIndexes[1]);
                    }

                    break;

                case ConnectionType.Enchanting:
                    if (CrossMenu)
                    {
                        //Check if double click TAKE ITEM
                        if (SwapCallers[PrioritizedIndex ^ 1] == ConnectedInventory)
                        {
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];

                            //Try to find an empty inventory slot
                            for (int ecx = 0; ecx < NormalInventory.Items.Length; ecx++)
                            {
                                if (NormalInventory.Items[ecx] == Item.None)
                                {
                                    NormalSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (NormalSlot == -1)
                                return;
                        }
                        //Check if double click GIVE ITEM
                        else if (SwapCallers[PrioritizedIndex] != ConnectedInventory)
                        {
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];
                            ConnectedSlot = 0; //Enchantment only has 1 slot -- take that one.
                        }

                        else //Standard swap
                        {
                            //Grab slot
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];
                        }

                        //Apply swap
                        StandardInventory.SwapItems(ConnectedInventory, ConnectedSlot, NormalInventory, NormalSlot);

                    }
                    else
                    {
                        //Just an Inventory <<>> Enemy (either direction)
                        StandardInventory.SwapItems(SwapCallers[0], StoredIndexes[0], SwapCallers[1], StoredIndexes[1]);
                    }

                    break;
                case ConnectionType.Storage:
                case ConnectionType.EnemyInventory:
                    if (CrossMenu)
                    {
                        //Check if double click TAKE ITEM
                        if (SwapCallers[PrioritizedIndex ^ 1] == ConnectedInventory)
                        {
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];

                            //Try to find an empty inventory slot
                            for (int ecx = 0; ecx < NormalInventory.Items.Length; ecx++)
                            {
                                if (NormalInventory.Items[ecx] == Item.None)
                                {
                                    NormalSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (NormalSlot == -1)
                                return;
                        }
                        //Check if double click GIVE ITEM
                        else if (SwapCallers[PrioritizedIndex] != ConnectedInventory)
                        {
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];

                            //Try to find an empty enemy slot
                            for (int ecx = 0; ecx < ConnectedInventory.Items.Length; ecx++)
                            {
                                if (ConnectedInventory.Items[ecx] == Item.None)
                                {
                                    ConnectedSlot = ecx;
                                    break;
                                }
                            }

                            //None found
                            if (ConnectedSlot == -1)
                                return;
                        }

                        else //Standard swap
                        {
                            //Grab slot
                            ConnectedSlot = StoredIndexes[PrioritizedIndex];
                            NormalSlot = StoredIndexes[PrioritizedIndex ^ 1];
                        }

                        //Apply swap
                        StandardInventory.SwapItems(ConnectedInventory, ConnectedSlot, NormalInventory, NormalSlot);

                    }
                    else
                    {
                        //Just an Inventory <<>> Enemy (either direction)
                        StandardInventory.SwapItems(SwapCallers[0], StoredIndexes[0], SwapCallers[1], StoredIndexes[1]);
                    }

                    break;

                default:
                    throw new Exception("Error finding proper connection in SwapHelper");
            }
        }


    }
}
