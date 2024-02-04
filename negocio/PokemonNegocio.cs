using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio1;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;

namespace negocio
{
	public class PokemonNegocio
	{
		public List<Pokemon> listar()
		{
			List<Pokemon> lista = new List<Pokemon>();

			SqlConnection conexion = new SqlConnection();
			SqlCommand comando = new SqlCommand();
			SqlDataReader lector;
			try
			{
				conexion.ConnectionString = "server = DESKTOP-S17IMP8\\SQLEXPRESS; database=POKEDEX_DB; integrated security = true";
				comando.CommandType = System.Data.CommandType.Text;
				comando.CommandText = "select Numero , Nombre , P.Descripcion , UrlImagen , E.Descripcion Tipo , D.Descripcion Debilidad , P.IdTipo , P.IdDebilidad , P.Id from POKEMONS P, ELEMENTOS E , ELEMENTOS D where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1";
				comando.Connection = conexion;
				conexion.Open();
				lector = comando.ExecuteReader();

				while (lector.Read())
				{
					Pokemon aux = new Pokemon();
					aux.Id = (int)lector["Id"];
					aux.Numero = lector.GetInt32(0);
					aux.Nombre = (string)lector["Nombre"];
					aux.Descripcion = (string)lector["Descripcion"];


					if (!(lector.IsDBNull(lector.GetOrdinal("urlImagen"))))
						aux.urlImagen = (string)lector["UrlImagen"];

					//if (!(lector["UrlImagen"] is DBNull))
					//    aux.urlImagen = (string)lector["UrlImagen"];


					aux.Tipo = new Elemento();
					aux.Tipo.Id = (int)lector["IdTipo"];
					aux.Tipo.Descripcion = (string)lector["Tipo"];
					aux.Debilidad = new Elemento();
					aux.Debilidad.Id = (int)lector["IdDebilidad"];
					aux.Debilidad.Descripcion = (string)lector["Debilidad"];
					lista.Add(aux);
				}

				conexion.Close();
				return lista;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		
        public void agregar(Pokemon nuevo)
        {
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.setearConsulta("Insert into POKEMONS (Numero , Nombre , Descripcion , Activo , IdTipo , IdDebilidad , UrlImagen) values (1" + nuevo.Numero + " , ' " + nuevo.Nombre + " ' , '" + nuevo.Descripcion + "' , 1 , @idTipo , @idDebilidad , @UrlImagen)");
				datos.setearParametro("@idTipo", nuevo.Tipo.Id);
				datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
				datos.setearParametro("@UrlImagen", nuevo.urlImagen);
				datos.ejecutarAccion();
			}
			catch (Exception ex)
			{

				throw ex;
			}
			finally
			{
				datos.cerrarConexion();
			}
        }
		public void modificar(Pokemon modificar)
		{
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.setearConsulta("update Pokemons set Numero = @numero , Nombre = @nombre , Descripcion = @descripcion , UrlImagen = @img , IdTipo = @idTipo , IdDebilidad = @idDebilidad where Id = @id");
				datos.setearParametro("@numero", modificar.Numero);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@img", modificar.urlImagen);
                datos.setearParametro("@idTipo", modificar.Tipo.Id);
                datos.setearParametro("@idDebilidad", modificar.Debilidad.Id);
                datos.setearParametro("@id", modificar.Id);

				datos.ejecutarAccion();
            }
			catch (Exception ex)
			{

				throw ex;
			}finally
			{
				datos.cerrarConexion();
			}
		}
		public List<Pokemon> filtrar(string campo , string criterio , string fitro)
		{
			List<Pokemon> lista = new List<Pokemon>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				string consulta = "select Numero , Nombre , P.Descripcion , UrlImagen , E.Descripcion Tipo , D.Descripcion Debilidad , P.IdTipo , P.IdDebilidad , P.Id from POKEMONS P, ELEMENTOS E , ELEMENTOS D where E.Id = P.IdTipo and D.Id = P.IdDebilidad And P.Activo = 1 And ";
				if (campo == "Número")
				{
                    switch (criterio)
                    {
                        case "Menor a ":
                            consulta += "Numero < " + fitro;
                            break;
                        case "Mayor a ":
                            consulta += "Numero > " + fitro;
                            break;
                        default:
                            consulta += "Numero = " + fitro;
                            break;
                    }
				}
				else if (campo == "Nombre")
				{
                    switch (criterio)
                    {
                        case "Comienza con ":
                            consulta += "Nombre like '" + fitro + "%' ";
                            break;
                        case "Termina con ":
                            consulta += "Nombre like '%" + fitro +"'" ;
                            break;
                        default:
                            consulta += "Nombre like '%"+ fitro +"%'";
                            break;
                    }
				}
				else
				{
                    switch (criterio)
                    {
                        case "Comienza con ":
                            consulta += "P.Descripcion like '" + fitro + "%' ";
                            break;
                        case "Termina con ":
                            consulta += "P.Descripcion like '%" + fitro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + fitro + "%'";
                            break;
                    }
                }
				datos.setearConsulta(consulta );
				datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];


                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("urlImagen"))))
                        aux.urlImagen = (string)datos.Lector["UrlImagen"];

                    //if (!(lector["UrlImagen"] is DBNull))
                    //    aux.urlImagen = (string)lector["UrlImagen"];


                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    lista.Add(aux);
                }
                return lista;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public void eliminar(int id)
		{
			try
			{
				AccesoDatos datos = new AccesoDatos();
				datos.setearConsulta("delete from pokemons where id = @id");
				datos.setearParametro("@id", id);
				datos.ejecutarAccion();
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public void eliminarLogico(int id)
		{
			try
			{
				AccesoDatos datos = new AccesoDatos();
				datos.setearConsulta("update POKEMONS set Activo = 0 where id = @id");
				datos.setearParametro("@id", id);
				datos.ejecutarAccion();
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
    }
}
