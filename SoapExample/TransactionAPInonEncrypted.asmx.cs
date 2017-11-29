using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Xml;

namespace SoapExample
{
    /// <summary>
    /// Summary description for TransactionAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TransactionAPInonEncrypted : System.Web.Services.WebService
    {
        public Credentials HeaderLogin;
        public Order DataOrder = new Order();


        private int GetBalance(string user)
        {
            try
            {
                
                    int balance;
                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("GetBalance", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@Username", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@UserBalance", SqlDbType.Int).Direction = ParameterDirection.Output;

                        command.Parameters["@Username"].Value = user;
                        command.ExecuteNonQuery();
                        balance = Convert.ToInt32(command.Parameters["@UserBalance"].Value);
                    }



                    return balance;
                
                
            }
            catch (Exception e)
            {
                throw  e;
            }
        }

        //overload method
        [WebMethod]
        [SoapHeader ("HeaderLogin", Direction =SoapHeaderDirection.In, Required = true)] 
        public int GetBalance()
        {
            try
            {
                if (CheckCredentials(HeaderLogin))
                {
                    int balance;
                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("GetBalance", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@Username", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@UserBalance", SqlDbType.Int).Direction = ParameterDirection.Output;

                        command.Parameters["@Username"].Value = HeaderLogin.username;
                        command.ExecuteNonQuery();
                        balance = Convert.ToInt32(command.Parameters["@UserBalance"].Value);
                    }



                    return balance;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                    SoapException.DetailElementName.Name,
                                                    SoapException.DetailElementName.Namespace);
                    // This is the detail part of the exception
                    detail.InnerText = "Wrong username/password";
                    throw new SoapException("Authentication failed",
                                            SoapException.ServerFaultCode,
                                            Context.Request.Url.AbsoluteUri, detail, null);
                }


            }
            catch (SoapException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                // Extract custom message from se.Detail.InnerText
                XmlDocument doc = new XmlDocument();
                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                SoapException.DetailElementName.Name,
                                                SoapException.DetailElementName.Namespace);
                // This is the detail part of the exception
                detail.InnerText = e.Message;
                throw new SoapException("Get Balance operation failed",
                                        SoapException.ServerFaultCode,
                                        Context.Request.Url.AbsoluteUri, detail, null);
            }
        }

        [WebMethod]
        [SoapHeader("HeaderLogin", Direction = SoapHeaderDirection.In, Required = true)]
        [SoapHeader("DataOrder",Direction = SoapHeaderDirection.Out, Required = true)]
        public List<Transaction> GetUserTransaction()
        {

           
            
            try
            {
                if (CheckCredentials(HeaderLogin))
                {
                    List<Transaction> transactions = new List<Transaction>();
                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("GetAllTransaction", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@username", SqlDbType.NVarChar, 50);
                        

                        command.Parameters["@Username"].Value = HeaderLogin.username;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Transaction tran = new Transaction
                                    {
                                        Amount = (int)reader[0],
                                        Performed_Transaction = reader[1].ToString()
                                    };
                                    transactions.Add(tran);

                                }
                               
                            }
                            else
                            {
                                XmlDocument doc = new XmlDocument();
                                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                                SoapException.DetailElementName.Name,
                                                                SoapException.DetailElementName.Namespace);
                                // This is the detail part of the exception
                                detail.InnerText = "No transactions found";
                                throw new SoapException("Transaction retrieval failure",
                                                        SoapException.ServerFaultCode,
                                                        Context.Request.Url.AbsoluteUri, detail, null);
                            }
                        }


                        DataOrder.OrderType = "Desceding";
                        return transactions;
                    }

                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                    SoapException.DetailElementName.Name,
                                                    SoapException.DetailElementName.Namespace);
                    // This is the detail part of the exception
                    detail.InnerText = "Wrong username/password";
                    throw new SoapException("Authentication failed",
                                            SoapException.ServerFaultCode,
                                            Context.Request.Url.AbsoluteUri, detail, null);
                }


            }
            catch (SoapException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                // Extract custom message from se.Detail.InnerText
                XmlDocument doc = new XmlDocument();
                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                SoapException.DetailElementName.Name,
                                                SoapException.DetailElementName.Namespace);
                // This is the detail part of the exception
                detail.InnerText = e.Message;
                throw new SoapException("Get Balance operation failed",
                                        SoapException.ServerFaultCode,
                                        Context.Request.Url.AbsoluteUri, detail, null);
            }
        }

        [WebMethod]
        [SoapHeader("HeaderLogin", Direction = SoapHeaderDirection.In, Required = true)]
        public Transaction GetLastTransaction()
        {
            try
            {
                if (CheckCredentials(HeaderLogin))
                {
                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("GetTransaction", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@username", SqlDbType.NVarChar, 50);
                        Transaction tran = new Transaction();

                        command.Parameters["@Username"].Value = HeaderLogin.username;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                tran.Amount = (int)reader[0];
                                tran.Performed_Transaction = reader[1].ToString();
                            }
                            else { 
                                XmlDocument doc = new XmlDocument();
                            XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                            SoapException.DetailElementName.Name,
                                                            SoapException.DetailElementName.Namespace);
                            // This is the detail part of the exception
                            detail.InnerText = "No transactions found";
                            throw new SoapException("Transaction retrieval failure",
                                                    SoapException.ServerFaultCode,
                                                    Context.Request.Url.AbsoluteUri, detail, null);
                            }
                        }
                        
                        
                        
                        return tran;
                    }

                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                    SoapException.DetailElementName.Name,
                                                    SoapException.DetailElementName.Namespace);
                    // This is the detail part of the exception
                    detail.InnerText = "Wrong username/password";
                    throw new SoapException("Authentication failed",
                                            SoapException.ServerFaultCode,
                                            Context.Request.Url.AbsoluteUri, detail, null);
                }
            
                    
            }
            catch (SoapException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                // Extract custom message from se.Detail.InnerText
                XmlDocument doc = new XmlDocument();
                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                SoapException.DetailElementName.Name,
                                                SoapException.DetailElementName.Namespace);
                // This is the detail part of the exception
                detail.InnerText = e.Message;
                throw new SoapException("Get Balance operation failed",
                                        SoapException.ServerFaultCode,
                                        Context.Request.Url.AbsoluteUri, detail, null);
            }

        }

        [WebMethod]
        [SoapHeader("HeaderLogin", Direction = SoapHeaderDirection.In, Required = true)]
        public void Deposit([XmlElement(IsNullable = true)] string user, int amount)
        {
            try
            {
                if (CheckCredentials(HeaderLogin))
                {

                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("Deposit_Amount", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@Username", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Amount", SqlDbType.Int);

                        command.Parameters["@Username"].Value = user;
                        command.Parameters["@Amount"].Value = amount;
                        command.ExecuteNonQuery();

                    }

                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                    SoapException.DetailElementName.Name,
                                                    SoapException.DetailElementName.Namespace);
                    // This is the detail part of the exception
                    detail.InnerText = "Wrong username/password";
                    throw new SoapException("Authentication failed",
                                            SoapException.ServerFaultCode,
                                            Context.Request.Url.AbsoluteUri, detail, null);
                }



            }
            catch (SoapException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                // Extract custom message from se.Detail.InnerText
                XmlDocument doc = new XmlDocument();
                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                SoapException.DetailElementName.Name,
                                                SoapException.DetailElementName.Namespace);
                // This is the detail part of the exception
                detail.InnerText = e.Message;
                throw new SoapException("Get Balance operation failed",
                                        SoapException.ServerFaultCode,
                                        Context.Request.Url.AbsoluteUri, detail, null);
            }
        }
    

        [WebMethod]
        [SoapHeader("HeaderLogin", Direction = SoapHeaderDirection.In, Required = true)]
        public void Withdraw([XmlElement(IsNullable = true)] string user, int amount)
        {
            try
            {
                if ((GetBalance(user) - amount)> 0){
                    string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                    using (var conn = new SqlConnection(connStr))
                    using (var command = new SqlCommand("Withdraw_Amount", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        conn.Open();
                        command.Parameters.Add("@Username", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Amount", SqlDbType.Int);

                        command.Parameters["@Username"].Value = user;
                        command.Parameters["@Amount"].Value = amount;
                        command.ExecuteNonQuery();

                    }

                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                    SoapException.DetailElementName.Name,
                                                    SoapException.DetailElementName.Namespace);
                    // This is the detail part of the exception
                    detail.InnerText = "insufficient funds";
                    throw new SoapException("Withdraw failure",
                                            SoapException.ServerFaultCode,
                                            Context.Request.Url.AbsoluteUri, detail, null);
                }


            }
            catch (SoapException se)
            {
                throw se;
            }
            catch (Exception e)
            {
                // Extract custom message from se.Detail.InnerText
                XmlDocument doc = new XmlDocument();
                XmlNode detail = doc.CreateNode(XmlNodeType.Element,
                                                SoapException.DetailElementName.Name,
                                                SoapException.DetailElementName.Namespace);
                // This is the detail part of the exception
                detail.InnerText = e.Message;
                throw new SoapException("Withdraw general exception",
                                        SoapException.ServerFaultCode,
                                        Context.Request.Url.AbsoluteUri, detail, null);
            }

        }


        private static bool CheckCredentials(Credentials crd)
        {
            try
            {
                string connStr = ConfigurationManager.AppSettings["myConnectionString"];
                using (var conn = new SqlConnection(connStr))
                using (var command = new SqlCommand("user_Login", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Pass", SqlDbType.NVarChar, 50);

                    command.Parameters["@Username"].Value = crd.username;
                    command.Parameters["@Pass"].Value = crd.password;
                    int result = (int)command.ExecuteScalar();
                    if (result == 1)
                        return true;
                    else
                        return false;

                }
                

                
            }
            catch
            {
                throw new Exception("Authorization problem");
            }
        }


    }
}
