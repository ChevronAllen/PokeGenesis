﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Media;
using NAudio.Vorbis;
using NAudio.Wave;
using GenesisDexEngine;


namespace GenesisDex
{
    public partial class FormMain : Form
    {
        PokemonList pokeXML = new PokemonList();
        List<Pokemon> pokeList = new List<Pokemon>();
        List<Mega> megaList = new List<Mega>();
        MegaList testMega = new MegaList();
        BanList banXML = new BanList();
        List<string> banList = new List<string>();
        List<Evolution> evoList = new List<Evolution>();
        List<Skill> skillList = new List<Skill>();
        List<Capability> capList = new List<Capability>();
        List<Moves> moveList = new List<Moves>();
        List<Ability> abiList = new List<Ability>();
        OptionsList optionsXML = new OptionsList();
        List<Options> optionsList = new List<Options>();
        List<Image> pokeImages = new List<Image>();
        List<Image> megaImages = new List<Image>();
        List<Image> megaxImages = new List<Image>();
        List<Image> megayImages = new List<Image>();
        List<string> updateList = new List<string>();
        int carryi { get; set; }
        int page = 1;
        int imageDisplayed = 0;
        int pbPokeLocX { get; set; }
        int pbPokeLocY { get; set; }
        bool mega { get; set; }
        bool megax { get; set; }
        bool viewMega { get; set; }
        bool onMegaX { get; set; }
        bool pokeChange { get; set; }
        bool done = false;
        List<string> pokeDex = new List<string>();
        bool dragging = false;
        Point dragCursorPoint;
        Point dragFormPoint;
        FormScan fs;

        public FormMain()
        {
            InitializeComponent();
            pbPokeLocX = pbPokemon.Location.X;
            pbPokeLocY = pbPokemon.Location.Y;
            imageDisplayed = 0;
            updateList.Add("updating...");
            pbY.Visible = false;
            pbX.Visible = false;
            pbPokeLeft.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeft.png");
            pbPokeRight.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRight.png");
            pbScan.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\ChangeMode.png");
            infoBack.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeft.png");
            infoForward.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRight.png");
            btnOptions.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\Options.png");
            btnCry.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\Cry.png");
            pbExit.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\CloseButton.png");
            btnMinimize.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MinimizeButton.png");
            RefreshPokedex();
        }

        private void lbPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pokeList == null) { return; }
            var i = 0;
            for (i = 0; i < pokeList.Count; i++)
            {
                try { if (lbPokemon.Text == pokeList[i].id.ToString()) { break; } } catch { }
            }
            pbY.Visible = false;
            pbX.Visible = false;
            try
            {
                megaList.Clear();
                megaList = testMega.createList("Mega" + pokeList[i].number);
                mega = megaList.Count() != 0;
            }
            catch { return; }
            try
            {
                megaList.Clear();
                megaList = testMega.createList("MegaX" + pokeList[i].number);
                megax = megaList.Count() != 0;
            }
            catch { return; }
            if (mega == true)
            {
                pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOff.PNG");
                viewMega = false;
            }
            else if (megax == true)
            {
                pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOff.PNG");
                onMegaX = false;
                viewMega = false;
            }
            else
            {
                pbY.Visible = false;
                pbX.Visible = false;
                pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaNo.PNG");

            }
            pokeImages.Clear();
            megaImages.Clear();
            megaxImages.Clear();
            megayImages.Clear();
            try
            {
                pokeImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + ".gif"));
            }
            catch { }
            int n = 0;
            done = false;
            while (done == false)
            {
                n++;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-" + n + ".gif"))
                {
                    pokeImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-" + n + ".gif"));
                }
                else
                {
                    done = true;
                }
            }
            try
            {
                pokeImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + ".gif"));
            }
            catch { return; }
            n = 0;
            done = false;
            while (done == false)
            {
                n++;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-" + n + ".gif"))
                {
                    pokeImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-" + n + ".gif"));
                }
                else
                {
                    done = true;
                }
            }
            if (mega == true)
            {
                megaImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega.gif"));
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-" + n + ".gif"))
                    {
                        megaImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
                megaImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega.gif"));
                n = 0;
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-" + n + ".gif"))
                    {
                        megaImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
            if (megax == true)
            {
                megaxImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-x.gif"));
                megayImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-y.gif"));
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-x-" + n + ".gif"))
                    {
                        megaxImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-x-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-y-" + n + ".gif"))
                    {
                        megayImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Pokemon\\" + pokeList[i].number + "-mega-y-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
                megaxImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-x.gif"));
                megayImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-y.gif"));
                n = 0;
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-x-" + n + ".gif"))
                    {
                        megaxImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-x-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
                done = false;
                while (done == false)
                {
                    n++;
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-y-" + n + ".gif"))
                    {
                        megayImages.Add(getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\Shiny\\" + pokeList[i].number + "-mega-y-" + n + ".gif"));
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
            viewMega = false;
            carryi = i;
            pbPokemon.Image = pokeImages[0];
            SetImage();
            imageDisplayed = 0;
            updatePage();
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void pbExit_MouseUp(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void pbExit_MouseEnter(object sender, EventArgs e)
        {
            pbExit.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\CloseButtonHover.PNG");
        }

        private void pbExit_MouseLeave(object sender, EventArgs e)
        {
            pbExit.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\CloseButton.PNG");
        }

        private void pbMega_MouseEnter(object sender, EventArgs e)
        {
            if (mega == true)
            {
                if (viewMega == true)
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOnHover.gif");
                }
                else
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOffHover.PNG");
                }
            }
        }

        private void pbMega_MouseLeave(object sender, EventArgs e)
        {
            if (mega == true)
            {
                if (viewMega == true)
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOn.gif");
                }
                else
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOff.PNG");
                }
            }
        }

        private void pbMega_MouseUp(object sender, MouseEventArgs e)
        {
            int i = carryi;
            if (mega == true)
            {
                if (viewMega == true)
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOffHover.PNG");
                    viewMega = false;
                    imageDisplayed = 0;
                    pbPokemon.Image = pokeImages[0];
                    SetImage();
                    updatePage();
                }
                else
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOnHover.gif");
                    viewMega = true;
                    imageDisplayed = 0;

                    pbPokemon.Image = megaImages[0];
                    SetImage();
                    updatePage();
                }
            }
            else if (megax == true)
            {
                if (viewMega == true)
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOffHover.PNG");
                    viewMega = false;
                    imageDisplayed = 0;
                    pbPokemon.Image = pokeImages[0];
                    SetImage();
                    pbY.Visible = false;
                    pbX.Visible = false;
                    updatePage();
                }
                else
                {
                    pbMega.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYesOnHover.gif");
                    changeMega();
                    viewMega = true;
                    pbY.Visible = true;
                    pbX.Visible = true;
                    updatePage();
                }
            }
        }

        private void changeMega()
        {
            int i = carryi;
            if (onMegaX == false)
            {
                onMegaX = true;
                imageDisplayed = 0;
                pbPokemon.Image = megaxImages[0];
                SetImage();
                pbY.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYOff.PNG");
                pbX.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaXOn.PNG");
                updatePage();
                return;
            }
            else if (onMegaX == true)
            {
                onMegaX = false;
                imageDisplayed = 0;
                pbPokemon.Image = megayImages[0];
                SetImage();
                pbY.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaYOn.PNG");
                pbX.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MegaXOff.PNG");
                updatePage();
                return;
            }
        }

        private void infoForward_MouseEnter(object sender, EventArgs e)
        {
            infoForward.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRightHover.PNG");
        }

        private void infoBack_MouseEnter(object sender, EventArgs e)
        {
            infoBack.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeftHover.PNG");
        }

        private void infoBack_MouseLeave(object sender, EventArgs e)
        {
            infoBack.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeft.PNG");
        }

        private void infoForward_MouseLeave(object sender, EventArgs e)
        {
            infoForward.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRight.PNG");
        }

        private void writeInfo()
        {
            int i = carryi;
            MegaList megaXML = new MegaList();
            if (viewMega == true)
            {
                if (megax == true)
                {
                    if (onMegaX == true)
                    {
                        megaList.Clear();
                        megaList = megaXML.createList("MegaX" + pokeList[i].number);
                        if (megaList[0].type == "Unchanged") { megaList[0].type = pokeList[i].type; }
                        rtbInfo.Text = string.Format(
                            "Number: {0}" + Environment.NewLine +
                            "Name: {1}" + Environment.NewLine +
                            "Type: {2}" + Environment.NewLine + Environment.NewLine +
                            "HP:      \t{3}\t{16}" + Environment.NewLine +
                            "ATK:     \t{4}\t{17}" + Environment.NewLine +
                            "DEF:     \t{5}\t{18}" + Environment.NewLine +
                            "SPATK:   \t{6}\t{19}" + Environment.NewLine +
                            "SPDEF:   \t{7}\t{20}" + Environment.NewLine +
                            "SPD:     \t{8}\t{21}" + Environment.NewLine + Environment.NewLine +
                            "Height: {9}" + Environment.NewLine +
                            "Weight: {10}" + Environment.NewLine + Environment.NewLine +
                            "Gender Ratio: {11}" + Environment.NewLine +
                            "Egg Group: {12}" + Environment.NewLine +
                            "Average Hatch Time: {13}" + Environment.NewLine + Environment.NewLine +
                            "Diet: {14}" + Environment.NewLine +
                            "Habitat: {15}",
                            pokeList[i].number, megaList[0].id, megaList[0].type, pokeList[i].hp, pokeList[i].atk, pokeList[i].def,
                            pokeList[i].spatk, pokeList[i].spdef, pokeList[i].spd, pokeList[i].size, pokeList[i].weight, pokeList[i].gender,
                            pokeList[i].egg, pokeList[i].hatch, pokeList[i].diet, pokeList[i].habitat, megaList[0].hp, megaList[0].atk,
                            megaList[0].def, megaList[0].spatk, megaList[0].spdef, megaList[0].spd);
                    }
                    else
                    {
                        megaList.Clear();
                        megaList = megaXML.createList("MegaY" + pokeList[i].number);
                        if (megaList[0].type == "Unchanged") { megaList[0].type = pokeList[i].type; }
                        rtbInfo.Text = string.Format(
                            "Number: {0}" + Environment.NewLine +
                            "Name: {1}" + Environment.NewLine +
                            "Type: {2}" + Environment.NewLine + Environment.NewLine +
                            "HP:      \t{3}\t{16}" + Environment.NewLine +
                            "ATK:     \t{4}\t{17}" + Environment.NewLine +
                            "DEF:     \t{5}\t{18}" + Environment.NewLine +
                            "SPATK:   \t{6}\t{19}" + Environment.NewLine +
                            "SPDEF:   \t{7}\t{20}" + Environment.NewLine +
                            "SPD:     \t{8}\t{21}" + Environment.NewLine + Environment.NewLine +
                            "Height: {9}" + Environment.NewLine +
                            "Weight: {10}" + Environment.NewLine + Environment.NewLine +
                            "Gender Ratio: {11}" + Environment.NewLine +
                            "Egg Group: {12}" + Environment.NewLine +
                            "Average Hatch Time: {13}" + Environment.NewLine + Environment.NewLine +
                            "Diet: {14}" + Environment.NewLine +
                            "Habitat: {15}",
                            pokeList[i].number, megaList[0].id, megaList[0].type, pokeList[i].hp, pokeList[i].atk, pokeList[i].def,
                            pokeList[i].spatk, pokeList[i].spdef, pokeList[i].spd, pokeList[i].size, pokeList[i].weight, pokeList[i].gender,
                            pokeList[i].egg, pokeList[i].hatch, pokeList[i].diet, pokeList[i].habitat, megaList[0].hp, megaList[0].atk,
                            megaList[0].def, megaList[0].spatk, megaList[0].spdef, megaList[0].spd);
                    }
                }
                else if (mega == true)
                {
                    megaList.Clear();
                    megaList = megaXML.createList("Mega" + pokeList[i].number);
                    if (megaList[0].type == "Unchanged") { megaList[0].type = pokeList[i].type; }
                    rtbInfo.Text = string.Format(
                        "Number: {0}" + Environment.NewLine +
                        "Name: {1}" + Environment.NewLine +
                        "Type: {2}" + Environment.NewLine + Environment.NewLine +
                        "HP:      \t{3}\t{16}" + Environment.NewLine +
                        "ATK:     \t{4}\t{17}" + Environment.NewLine +
                        "DEF:     \t{5}\t{18}" + Environment.NewLine +
                        "SPATK:   \t{6}\t{19}" + Environment.NewLine +
                        "SPDEF:   \t{7}\t{20}" + Environment.NewLine +
                        "SPD:     \t{8}\t{21}" + Environment.NewLine + Environment.NewLine +
                        "Height: {9}" + Environment.NewLine +
                        "Weight: {10}" + Environment.NewLine + Environment.NewLine +
                        "Gender Ratio: {11}" + Environment.NewLine +
                        "Egg Group: {12}" + Environment.NewLine +
                        "Average Hatch Time: {13}" + Environment.NewLine + Environment.NewLine +
                        "Diet: {14}" + Environment.NewLine +
                        "Habitat: {15}",
                        pokeList[i].number, megaList[0].id, megaList[0].type, pokeList[i].hp, pokeList[i].atk, pokeList[i].def,
                        pokeList[i].spatk, pokeList[i].spdef, pokeList[i].spd, pokeList[i].size, pokeList[i].weight, pokeList[i].gender,
                        pokeList[i].egg, pokeList[i].hatch, pokeList[i].diet, pokeList[i].habitat, megaList[0].hp, megaList[0].atk, 
                        megaList[0].def, megaList[0].spatk, megaList[0].spdef, megaList[0].spd);
                }
            }
            else
            {
                rtbInfo.Text = string.Format(
                    "Number: {0}" + Environment.NewLine +
                    "Name: {1}" + Environment.NewLine +
                    "Type: {2}" + Environment.NewLine + Environment.NewLine +
                    "HP:      \t{3}" + Environment.NewLine +
                    "ATK:     \t{4}" + Environment.NewLine +
                    "DEF:     \t{5}" + Environment.NewLine +
                    "SPATK:   \t{6}" + Environment.NewLine +
                    "SPDEF:   \t{7}" + Environment.NewLine +
                    "SPD:     \t{8}" + Environment.NewLine + Environment.NewLine +
                    "Height: {9}" + Environment.NewLine +
                    "Weight: {10}" + Environment.NewLine + Environment.NewLine +
                    "Gender Ratio: {11}" + Environment.NewLine +
                    "Egg Group: {12}" + Environment.NewLine +
                    "Average Hatch Time: {13}" + Environment.NewLine + Environment.NewLine +
                    "Diet: {14}" + Environment.NewLine +
                    "Habitat: {15}",
                    pokeList[i].number, pokeList[i].id, pokeList[i].type, pokeList[i].hp, pokeList[i].atk, pokeList[i].def,
                    pokeList[i].spatk, pokeList[i].spdef, pokeList[i].spd, pokeList[i].size, pokeList[i].weight, pokeList[i].gender,
                    pokeList[i].egg, pokeList[i].hatch, pokeList[i].diet, pokeList[i].habitat);
            }
        }

        private void writeStats()
        {
            int i = carryi;
            CapabilityList capXML = new CapabilityList();
            SkillList skillXML = new SkillList();
            capList.Clear();
            skillList.Clear();
            capList = capXML.createList(pokeList[i].number);
            skillList = skillXML.createList(pokeList[i].number);
            rtbInfo.Text = ("Capabilities:" + Environment.NewLine);
            for (var e = 0; e < capList.Count; e++)
            {
                rtbInfo.Text += "-" + capList[e].cap + Environment.NewLine;
            }
            rtbInfo.Text += Environment.NewLine + "Skills:" + Environment.NewLine;
            for (var e = 0; e < skillList.Count; e++)
            {
                rtbInfo.Text += "-" + skillList[e].name+ " " + skillList[e].die + "d6+" + skillList[e].bonus + Environment.NewLine;
            }

        }

        private void writeMoves()
        {
            int i = carryi;
            MovesList moveXML = new MovesList();
            moveList.Clear();
            moveList = moveXML.createList(pokeList[i].number);
            rtbInfo.Text = ("Moves:" + Environment.NewLine);
            for (var e = 0; e < moveList.Count; e++)
            {
                rtbInfo.Text += "-" + moveList[e].move + Environment.NewLine;
            }
        }

        private void writeEvo()
        {
            int i = carryi;
            EvolutionList evoXML = new EvolutionList();
            evoList.Clear();
            evoList = evoXML.createList(pokeList[i].number);
            rtbInfo.Text = ("Evolutions:" + Environment.NewLine);
            for (var e = 0; e < evoList.Count; e++)
            {
                rtbInfo.Text += "-" + evoList[e].evo + Environment.NewLine;
            }
            BasicAbiList basicXML = new BasicAbiList();
            AdvAbiList advXML = new AdvAbiList();
            HighAbiList highXML = new HighAbiList();
            abiList.Clear();
            abiList = basicXML.createList(pokeList[i].number);
            rtbInfo.Text += Environment.NewLine + "Abilities:";
            for (var s = 0; s < abiList.Count; s++)
            {
                rtbInfo.Text += Environment.NewLine + "Basic Ability - " + abiList[s].basicability;
            }
            abiList = advXML.createList(pokeList[i].number);
            for (var s = 0; s < abiList.Count; s++)
            {
                rtbInfo.Text += Environment.NewLine + "Advanced Ability - " + abiList[s].advability;
            }
            abiList = highXML.createList(pokeList[i].number);
            for (var s = 0; s < abiList.Count; s++)
            {
                rtbInfo.Text += Environment.NewLine + "High Ability - " + abiList[s].highability;
            }
            MegaList megaAbility = new MegaList();
            if (viewMega == true)
            {
                if (mega == true)
                {
                    megaList = megaAbility.createList("Mega" + pokeList[i].number);
                    rtbInfo.Text += string.Format(Environment.NewLine + "Mega Ability - " + megaList[0].ability);
                }
                else if (megax == true)
                {
                    if (onMegaX == true)
                    {
                        megaList = megaAbility.createList("MegaX" + pokeList[i].number);
                        rtbInfo.Text += string.Format(Environment.NewLine + "Mega Ability X - " + megaList[0].ability);
                    }
                    else
                    {
                        megaList = megaAbility.createList("MegaY" + pokeList[i].number);
                        rtbInfo.Text += string.Format(Environment.NewLine + "Mega Ability Y - " + megaList[0].ability);
                    }
                }
            }
        }

        private void infoForward_MouseUp(object sender, MouseEventArgs e)
        {
            page += 1;
            updatePage();

        }

        private void infoBack_MouseUp(object sender, MouseEventArgs e)
        {
            page -= 1;
            updatePage();
        }

        private void updatePage()
        {
            if (page == 5) { page = 1; }
            if (page == 0) { page = 4; }
            tbPageCount.Text = page.ToString() + "/4";
            if (page == 1) { writeInfo(); }
            if (page == 2) { writeStats(); }
            if (page == 3) { writeMoves(); }
            if (page == 4) { writeEvo(); }
            rtbEntry.Text = pokeList[carryi].entry;
        }

        private void pbPokeRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (viewMega)
            {
                if (!megax)
                {
                    imageDisplayed += 1;
                    if (imageDisplayed >= pokeImages.Count) { imageDisplayed = 0; }
                    pbPokemon.Image = megaImages[imageDisplayed];
                }
                else
                {
                    if (onMegaX)
                    {
                        imageDisplayed += 1;
                        if (imageDisplayed >= pokeImages.Count) { imageDisplayed = 0; }
                        pbPokemon.Image = megaxImages[imageDisplayed];
                    }
                    else
                    {
                        imageDisplayed += 1;
                        if (imageDisplayed >= pokeImages.Count) { imageDisplayed = 0; }
                        pbPokemon.Image = megayImages[imageDisplayed];
                    }
                }
            }
            else
            {
                imageDisplayed += 1;
                if (imageDisplayed >= pokeImages.Count) { imageDisplayed = 0; }
                pbPokemon.Image = pokeImages[imageDisplayed];
            }
            SetImage();
        }
        private void pbPokeLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (viewMega)
            {
                if (!megax)
                {
                    if (imageDisplayed == 0) { imageDisplayed = pokeImages.Count - 1; }
                    else { imageDisplayed -= 1; }
                    pbPokemon.Image = megaImages[imageDisplayed];
                }
                else
                {
                    if (onMegaX)
                    {
                        if (imageDisplayed == 0) { imageDisplayed = pokeImages.Count - 1; }
                        else { imageDisplayed -= 1; }
                        pbPokemon.Image = megaxImages[imageDisplayed];
                    }
                    else
                    {
                        if (imageDisplayed == 0) { imageDisplayed = pokeImages.Count - 1; }
                        else { imageDisplayed -= 1; }
                        pbPokemon.Image = megayImages[imageDisplayed];
                    }
                }
            }
            else
            {
                if (imageDisplayed == 0) { imageDisplayed = pokeImages.Count - 1; }
                else { imageDisplayed -= 1; }
                pbPokemon.Image = pokeImages[imageDisplayed];
            }
            SetImage();
        }

        private void pbX_MouseUp(object sender, MouseEventArgs e)
        {
            if (onMegaX == false)
            {
                changeMega();
            }
        }

        private void pbY_MouseUp(object sender, MouseEventArgs e)
        {
            if (onMegaX == true)
            {
                changeMega();
            }
        }

        private void RefreshPokedex()
        {
            pokeList = new List<Pokemon>();
            banList = new List<string>();
            optionsList = new List<Options>();
            optionsList = optionsXML.createList();
            this.BackgroundImage = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MainMenu" + optionsList[0].PokedexSkin + ".PNG");
            pokeDex.Clear();
            pokeList.Clear();
            banList.Clear();
            pokeList = pokeXML.createList();
            banList = banXML.createList();
            for (int p = 0; p < pokeList.Count; p++ )
            {
                if (banList.Contains(pokeList[p].id))
                {
                    pokeList.RemoveAt(p);
                    p--;
                }
            }
            SortPokeList();
            for (var i = 0; i < pokeList.Count; i++)
            {
                pokeDex.Add(pokeList[i].id);
            }
            lbPokemon.DataSource = updateList;
            lbPokemon.DataSource = pokeDex;
            lbPokemon.SelectedIndex = 0;
            lbPokemon.Refresh();
        }

        private Image getImage(string x)
        {
            string path = (x);
            if (File.Exists(x) == true)
            {
                return Image.FromFile(path);
            }
            else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\MissingNo.gif"))
            {
                return Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Data\\Images\\MissingNo.gif");
            }
            else
            {
                return Image.FromFile(null);
            }
        }

        private void SortPokeList()
        {
            pokeList.Sort(delegate (Pokemon x, Pokemon y)
            {
                try { return x.number.CompareTo(y.number); } catch { return 0; }
            });
        }

        private void pbScan_MouseUp(object sender, MouseEventArgs e)
        {
            if (fs == null)
            {
                fs = new FormScan();
                fs.FormClosed += fs_FormClosed;
            }
            fs.Show(this);
            this.Activated += fm_FormActive;
            Hide();
        }

        private void fm_FormActive(object sender, EventArgs e)
        {
            RefreshPokedex();
        }

        private void fs_FormClosed(object sender, FormClosedEventArgs e)
        {
            fs = null;
            Show();
        }

        private void pbScan_MouseEnter(object sender, EventArgs e)
        {
            pbScan.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\ChangeModeHover.png");
        }

        private void pbScan_MouseLeave(object sender, EventArgs e)
        {
            pbScan.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\ChangeMode.png");
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                List<string> searchDex = new List<string>();
                searchDex.Clear();
                foreach (string s in pokeDex)
                {
                    if (s.StartsWith(tbSearch.Text, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        searchDex.Add(s);
                    }
                }
                lbPokemon.DataSource = searchDex;
            }
            else
            {
                lbPokemon.DataSource = updateList;
                lbPokemon.DataSource = pokeDex;
            }
        }

        private void tbSearch_Click(object sender, EventArgs e)
        {
            if (tbSearch.Text != "")
            {
                tbSearch.Text = "";
            }
        }

        private void SetImage()
        {
            pbPokemon.Size = pbPokemon.Image.Size;
            pbPokemon.Location = new Point(189 - (pbPokemon.Width / 2), 250 - (pbPokemon.Height));
        }

        private void btnOptions_MouseUp(object sender, MouseEventArgs e)
        {
            this.Enabled = false;
            FormOptions fc = new FormOptions();
            fc.FormClosing += FormOptionsIsClosing;
            fc.Show();
        }

        private void FormOptionsIsClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }
            this.Enabled = true;
            this.Show();
            RefreshPokedex();
            this.Update();
        }

        private void btnOptions_MouseEnter(object sender, EventArgs e)
        {
            btnOptions.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\OptionsHover.png");
        }

        private void btnOptions_MouseLeave(object sender, EventArgs e)
        {
            btnOptions.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\Options.png");

        }

        private void btnCry_MouseUp(object sender, MouseEventArgs e)
        {
            string s;
            if (optionsList[0].CryVolume != 10)
            {
                s = "0." + optionsList[0].CryVolume.ToString();
            }
            else
            {
                s = "1.0";
            }
            float f;
            Single.TryParse(s, out f);
            if (viewMega)
            {
                if (!megax)
                {
                    var CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Empty.ogg");
                    try { CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Cries\\" + pokeList[carryi].number + "-mega.ogg"); } catch { MessageBox.Show(pokeList[carryi].number + "-mega.ogg does not exist."); }
                    var CryPlay = new WaveOut();
                    CryPlay.Init(CryOGG);
                    CryPlay.Volume = f;
                    CryPlay.Play();
                }
                else
                {
                    if (onMegaX)
                    {
                        var CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Empty.ogg");
                        try { CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Cries\\" + pokeList[carryi].number + "-mega-x.ogg"); } catch { MessageBox.Show(pokeList[carryi].number + "-mega-x.ogg does not exist."); }
                        var CryPlay = new WaveOut();
                        CryPlay.Init(CryOGG);
                        CryPlay.Volume = f;
                        CryPlay.Play();
                    }
                    else
                    {
                        var CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Empty.ogg");
                        try { CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Cries\\" + pokeList[carryi].number + "-mega-y.ogg"); } catch { MessageBox.Show(pokeList[carryi].number + "-mega-y.ogg does not exist."); }
                        var CryPlay = new WaveOut();
                        CryPlay.Init(CryOGG);
                        CryPlay.Volume = f;
                        CryPlay.Play();
                    }
                }
            }
            else
            {
                var CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Empty.ogg");
                try {CryOGG = new VorbisWaveReader(AppDomain.CurrentDomain.BaseDirectory + "Data\\Audio\\Cries\\" + pokeList[carryi].number + ".ogg"); } catch { MessageBox.Show(pokeList[carryi].number + ".ogg does not exist."); }
                var CryPlay = new WaveOut();
                CryPlay.Init(CryOGG);
                CryPlay.Volume = f;
                CryPlay.Play();
            }
        }

        private void btnCry_MouseEnter(object sender, EventArgs e)
        {
            btnCry.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\CryHover.png");
        }
        private void btnCry_MouseLeave(object sender, EventArgs e)
        {
            btnCry.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\Cry.png");
        }

        private void pbPokeRight_MouseEnter(object sender, EventArgs e)
        {
            pbPokeRight.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRightHover.png");
        }
        private void pbPokeRight_MouseLeave(object sender, EventArgs e)
        {
            pbPokeRight.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoRight.png");
        }

        private void pbPokeLeft_MouseEnter(object sender, EventArgs e)
        {
            pbPokeLeft.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeftHover.png");
        }
        private void pbPokeLeft_MouseLeave(object sender, EventArgs e)
        {
            pbPokeLeft.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\InfoLeft.png");
        }

        //===========================================================================================================
        //=== 
        //===========================================================================================================
        private void btnMinimize_MouseUp(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MinimizeButtonHover.png");
        }
        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.Image = getImage(AppDomain.CurrentDomain.BaseDirectory + "Data\\GUI\\MinimizeButton.png");
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
