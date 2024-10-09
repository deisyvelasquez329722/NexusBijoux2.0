using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GymApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string _connectionString;

        public UsuarioController(IConfiguration configuration)
        {
            // Obtiene la cadena de conexión desde appsettings.json
            _connectionString = configuration.GetConnectionString("MyConnection");
        }

        // Verificar Usuario (Login)
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return Json(new { status = "Error", message = "Los campos están incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Consulta para verificar si el usuario existe
                    string query = "SELECT Password FROM Usuario WHERE Username = @user";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user", user);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string storedPassword = reader["Password"].ToString();

                            if (password != storedPassword)
                            {
                                return Json(new { status = "Error", message = "Error en la contraseña" });
                            }

                            return Json(new { status = "ok", message = "Usuario loggeado", redirect = "/inventario" });
                        }
                        else
                        {
                            return Json(new { status = "Error", message = "Usuario no encontrado" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Login", "Usuario");
            }
        }

        // Crear nueva cuenta (Registro)
        [HttpPost]
        public ActionResult CrearCuenta(string user, string password, string email)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                return Json(new { status = "Error", message = "Los campos están incompletos" });
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Verificar si el usuario ya existe
                    string checkUserQuery = "SELECT COUNT(*) FROM Usuario WHERE Username = @user OR Email = @Email";
                    using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@user", user);
                        checkCommand.Parameters.AddWithValue("@Email", email);

                        int userExists = (int)checkCommand.ExecuteScalar();

                        if (userExists > 0)
                        {
                            return Json(new { status = "Error", message = "El usuario o correo ya está en uso" });
                        }
                    }

                    // Insertar nuevo usuario
                    string insertQuery = "INSERT INTO Usuario (Username, Password, Email) VALUES (@user, @password, @Email)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@user", user);
                        insertCommand.Parameters.AddWithValue("@password", password); // Considera hashear la contraseña
                        insertCommand.Parameters.AddWithValue("@Email", email);

                        int result = insertCommand.ExecuteNonQuery();

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
                Console.WriteLine(ex.Message);
                return Json(new { status = "Error", message = "Ocurrió un error en el servidor" });
            }
        }
    }
}


