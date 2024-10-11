using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchivosFTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            // Crear un cuadro de diálogo para seleccionar un archivo
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Mostrar la ruta del archivo seleccionado en el TextBox
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            // Verificar si se ha seleccionado un archivo
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                lblStatus.Text = "Por favor, selecciona un archivo.";
                return;
            }

            // Configurar los detalles del servidor FTP
            string ftpServer = "192.168.18.74";    // Reemplaza con tu servidor FTP
            string ftpUsername = "admin";          // Reemplaza con tu usuario FTP
            string ftpPassword = "123";       // Reemplaza con tu contraseña FTP
            string remoteFilePath = @"/"; // Ruta remota en el servidor FTP

            try
            {
                lblStatus.Text = "Subiendo archivo...";

                // Crear un nuevo cliente FTP
                using (var ftpClient = new FtpClient(ftpServer))
                {
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    // Conectar al servidor FTP
                    ftpClient.Connect();

                    // Subir el archivo seleccionado
                    string localFilePath = txtFilePath.Text;
                    string remoteFileName = Path.GetFileName(localFilePath); // Nombre del archivo en el servidor

                    FtpStatus uploadResult = ftpClient.UploadFile(localFilePath, remoteFilePath + remoteFileName);

                    if (uploadResult == FtpStatus.Success)
                    {
                        lblStatus.Text = "Archivo subido exitosamente.";
                    }
                    else
                    {
                        lblStatus.Text = "Error al subir el archivo.";
                    }

                    // Desconectar del servidor FTP
                    ftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }
    }
}
