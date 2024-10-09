using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace GymApp.Controllers
{
    public class UsuarioController : Controller
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la configuración para obtener la cadena de conexión
        public UsuarioController(IConfiguration configuration)
        {
            // Obtener la cadena de conexión desde appsettings.json
            _connectionString = configuration.GetConnectionString("MyConnection");
        }

        // Método para verificar el login de un usuario
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return Json(new { status = "Error", message = "Los campos están incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Consulta SQL para verificar si el usuario existe
                    string query = "SELECT Password FROM Usuario WHERE Username = @user";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Añadir el parámetro de usuario
                        command.Parameters.AddWithValue("@user", user);

                        // Ejecutar la consulta
                        SqlDataReader reader = command.ExecuteReader();

                        // Leer el resultado
                        if (reader.Read())
                        {
                            // Obtener la contraseña almacenada
                            string storedPassword = reader["Password"].ToString();

                            // Comparar la contraseña proporcionada con la almacenada
                            if (password != storedPassword) // Asegúrate de que se use hashing
                            {
                                return Json(new { status = "Error", message = "Error en la contraseña" });
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

        // Método para crear una nueva cuenta
        [HttpPost]
        public ActionResult CrearCuenta(string user, string password, string email)
        {
            // Verificar que todos los campos estén completos
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                return Json(new { status = "Error", message = "Los campos están incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Verificar si el usuario o el email ya existen
                    string checkUserQuery = "SELECT COUNT(*) FROM Usuario WHERE Username = @user OR Email = @Email";
                    using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        // Añadir parámetros de usuario y email
                        checkCommand.Parameters.AddWithValue("@user", user);
                        checkCommand.Parameters.AddWithValue("@Email", email);

                        // Ejecutar la consulta y obtener el conteo
                        int userExists = (int)checkCommand.ExecuteScalar();

                        // Si el usuario o el email ya existen, devolver un error
                        if (userExists > 0)
                        {
                            return Json(new { status = "Error", message = "El usuario o correo ya está en uso" });
                        }
                    }

                    // Insertar un nuevo usuario en la base de datos
                    string insertQuery = "INSERT INTO Usuario (Username, Password, Email) VALUES (@user, @password, @Email)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        // Añadir parámetros para la inserción
                        insertCommand.Parameters.AddWithValue("@user", user);
                        insertCommand.Parameters.AddWithValue("@password", password); // Considera hashear la contraseña
                        insertCommand.Parameters.AddWithValue("@Email", email);

                        // Ejecutar la inserción
                        int result = insertCommand.ExecuteNonQuery();

                        // Verificar si la inserción fue exitosa
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
                return Json(new { status = "Error", message = "Ocurrió un error en el servidor" });
            }
        }
    }
}
