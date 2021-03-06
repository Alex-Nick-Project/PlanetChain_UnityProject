﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Identicon{

    private Int32[] randseed = new Int32[4];
    private Color[] iconPixels;
    private int Size;


    public Identicon(string Seed, int size)
    {
        Size = size;
        createImageData(Seed);
    }


    public Texture2D GetBitmap(int resolution)
    {
        int Scale = resolution / Size;
        return CreateEthereumIcon(Scale);
    }



    private void seedrand(string seed)
    {
        char[] seedArray = seed.ToCharArray();
        for (int i = 0; i < randseed.Length; i++)
            randseed[i] = 0;
        for (int i = 0; i < seed.Length; i++)
            randseed[i % 4] = ((randseed[i % 4] << 5) - randseed[i % 4]) + seedArray[i];
    }

    private double rand()
    {
        var t = randseed[0] ^ (randseed[0] << 11);

        randseed[0] = randseed[1];
        randseed[1] = randseed[2];
        randseed[2] = randseed[3];
        randseed[3] = (randseed[3] ^ (randseed[3] >> 19) ^ t ^ (t >> 8));
        return Convert.ToDouble(randseed[3]) / Convert.ToDouble((System.UInt32)1 << 31);
    }

    private double hue2rgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1D / 6) return p + (q - p) * 6 * t;
        if (t < 1D / 2) return q;
        if (t < 2D / 3) return p + (q - p) * (2D / 3 - t) * 6;
        return p;
    }

    private Color HSLtoRGB(double h, double s, double l)
    {
        double r, g, b;
        if (s == 0)
        {
            r = g = b = l; 
        }
        else
        {
            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            var p = 2 * l - q;
            r = hue2rgb(p, q, h + 1D / 3);
            g = hue2rgb(p, q, h);
            b = hue2rgb(p, q, h - 1D / 3);
        }
        Color IconColor=new Color((float)r, (float)g, (float)b, 1);
        return IconColor;
    }

    private Color createColor()
    {
        var h = (rand());
        var s = ((rand() * 0.6) + 0.4);
        var l = ((rand() + rand() + rand() + rand()) * 0.25);
        return HSLtoRGB(h, s, l);
    }

    private void createImageData(string seed)
    {
        seedrand(seed.ToLower());
        var mainColor = createColor();
        var bgColor = createColor();
        var spotColor = createColor();

        int width = Size;
        int height = Size;

        int mirrorWidth = width / 2;
        int dataWidth = width - mirrorWidth;
        double[] data = new double[width * height];
        for (int y = 0; y < height; y++)
        {
            double[] row = new double[dataWidth];
            for (int x = 0; x < dataWidth; x++)
            {
                row[x] = Math.Floor(rand() * 2.3);
            }
            Array.Copy(row, 0, data, y * width, dataWidth);
            Array.Copy(row.Reverse().ToArray(), 0, data, y * width + dataWidth, mirrorWidth);
        }

        iconPixels = new Color[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == 1)
            {
                iconPixels[i] = mainColor;
            }
            else if (data[i] == 0)
            {
                iconPixels[i] = bgColor;
            }
            else
            {
                iconPixels[i] = spotColor;
            }
        }
    }

    private Texture2D CreateEthereumIcon(int scale)
    {

        Texture2D pic = new Texture2D(Size*scale,Size*scale);

        for (int i = 0; i < iconPixels.Length; i++)
        {
            int x = i % Size;
            int y = i / Size;
            for (int xx = x * scale; xx < x * scale + scale; xx++)
            {
                for (int yy = y * scale; yy < y * scale + scale; yy++)
                {
                    pic.SetPixel(xx, yy, iconPixels[i]);
                }
            }
        }
        return pic;
    }
}
