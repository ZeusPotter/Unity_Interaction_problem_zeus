using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    // Convierte AudioClip a WAV bytes en memoria
    public static byte[] FromAudioClip(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();
        int headerSize = 44;
        // Write the header with placeholder sizes (will be fixed later)
        stream.Position = 0;
        WriteHeader(stream, clip, headerSize);

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        short[] intData = new short[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        // Convertir float [-1,1] a short [-32768, 32767]
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * short.MaxValue);
            byte[] byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        stream.Write(bytesData, 0, bytesData.Length);

        // Volver a escribir el header con tamaños correctos
        stream.Position = 0;
        WriteHeader(stream, clip, headerSize);

        return stream.ToArray();
    }

    private static void WriteHeader(Stream stream, AudioClip clip, int headerSize)
    {
        int fileSize = (clip.samples * clip.channels * 2) + headerSize - 8;
        int fmtChunkSize = 16;
        short audioFormat = 1; // PCM
        short numChannels = (short)clip.channels;
        int sampleRate = clip.frequency;
        short bitsPerSample = 16;
        short blockAlign = (short)(numChannels * bitsPerSample / 8);
        int byteRate = sampleRate * blockAlign;

        BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write(fileSize);
        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[] { 'f', 'm', 't', ' ' });
        writer.Write(fmtChunkSize);
        writer.Write(audioFormat);
        writer.Write(numChannels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write(blockAlign);
        writer.Write(bitsPerSample);
        writer.Write(new char[] { 'd', 'a', 't', 'a' });
        writer.Write(fileSize - 36);
    }
}
