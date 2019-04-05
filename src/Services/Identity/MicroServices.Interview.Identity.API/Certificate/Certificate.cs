﻿using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace MicroServices.Interview.Identity.API.Certificates
{
    internal static class Certificate
    {
        public static X509Certificate2 Get()
        {
            var assembly = typeof(Certificate).GetTypeInfo().Assembly;
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            /*
             * Test amaçlı sertifia oluşturuyoruz
             * sertifikayı Embedded resource olarak ekledik o yüzden namespace altından alıyoruz
             */
            using (var stream = assembly.GetManifestResourceStream("MicroServices.Interview.Identity.API.Certificate.idsrv3test.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}