using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace GymApp.Controllers
{
    public class UsuarioController : Controller
    {
        // Cadena de conexi�n a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la configuraci�n para obtener la cadena de conexi�n
        public UsuarioController(IConfiguration configuration)
        {
            // Obtener la cadena de conexi�n desde appsettings.json
            _connectionString = configuration.GetConnectionString("MyConnection");
        }

        // M�todo para verificar el login de un usuario
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            // Verificar que los campos no est�n vac�os
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return Json(new { status = "Error", message = "Los campos est�n incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Abrir la conexi�n a la base de datos
                    connection.Open();

                    // Consulta SQL para verificar si el usuario existe
                    string query = "SELECT Password FROM Usuario WHERE Username = @user";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // A�adir el par�metro de usuario
                        command.Parameters.AddWithValue("@user", user);

                        // Ejecutar la consulta
                        SqlDataReader reader = command.ExecuteReader();

                        // Leer el resultado
                        if (reader.Read())
                        {
                            // Obtener la contrase�a almacenada
                            string storedPassword = reader["Password"].ToString();

                            // Comparar la contrase�a proporcionada con la almacenada
                            if (password != storedPassword) // Aseg�rate de que se use hashing
                            {
                                return Json(new { status = "Error", message = "Error en la contrase�a" });
                            }

                            // Login exitoso
                            return Json(new { status = "ok", message = "Usuario loggeado", redirect = "/inventario" });
                        }
                        else
                        {
                            // Usuario no encontrado
                            return Json(new { status = "Error", message = "Usuario no encontrado" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine(ex.Message);
                return RedirectToAction("Login", "Usuario");
            }
        }

        // M�todo para crear una nueva cuenta
        [HttpPost]
        public ActionResult CrearCuenta(string user, string password, string email)
        {
            // Verificar que todos los campos est�n completos
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                return Json(new { status = "Error", message = "Los campos est�n incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Abrir la conexi�n a la base de datos
                    connection.Open();

                    // Verificar si el usuario o el email ya existen
                    string checkUserQuery = "SELECT COUNT(*) FROM Usuario WHERE Username = @user OR Email = @Email";
                    using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        // A�adir par�metros de usuario y email
                        checkCommand.Parameters.AddWithValue("@user", user);
                        checkCommand.Parameters.AddWithValue("@Email", email);

                        // Ejecutar la consulta y obtener el conteo
                        int userExists = (int)checkCommand.ExecuteScalar();

                        // Si el usuario o el email ya existen, devolver un error
                        if (userExists > 0)
                        {
                            return Json(new { status = "Error", message = "El usuario o correo ya est� en uso" });
                        }
                    }

                    // Insertar un nuevo usuario en la base de datos
                    string insertQuery = "INSERT INTO Usuario (Username, Password, Email) VALUES (@user, @password, @Email)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        // A�adir par�metros para la inserci�n
                        insertCommand.Parameters.AddWithValue("@user", user);
                        insertCommand.Parameters.AddWithValue("@password", password); // Considera hashear la contrase�a
                        insertCommand.Parameters.AddWithValue("@Email", email);

                        // Ejecutar la inserci�n
                        int result = insertCommand.ExecuteNonQuery();

                        // Verificar si la inserci�n fue exitosa
                        if (result > 0)
                        {
                            return Json(new { status = "ok", message = "Cuenta creada exitosamente", redirect = "/inventario" });
                        }
                        else
                        {
                            return Json(new { status = "Error", message = "Error al crear la cuenta" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine(ex.Message);
                return Json(new { status = "Error", message = "Ocurri� un error en el servidor" });
            }
        }
    }
}
