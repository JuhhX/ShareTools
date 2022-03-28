using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Tcc2._1.App_Start;
using Tcc2._1.DAL;
using Tcc2._1.Models;
using System.Net.Mail;
using System.Net;


namespace Tcc2._1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel usuario)
        {
            //Checa se oq foi recebido na variavel usuario é valido
            if (ModelState.IsValid)
            {
                //Variavel para referenciar as "tabelas" ou banco de dados
                var db = new ToolsContext();
                //A senha recebida é transformada em Hash para comparar depois
                var senha = Usuario.GerarHash(usuario.Senha);
                //Consultar se os dados estão de acordo com o Banco de dados
                var user = db.Usuario.FirstOrDefault(l => (l.nome_User == usuario.Email || l.email_User == usuario.Email) && (l.senha == senha));

                //Se o Usuario for encontrado
                if (user != null && user.senha.Equals(Usuario.GerarHash(usuario.Senha)))
                {
                    //Criando uma identidade e armazenando os dados do usuário
                    var identity = new ClaimsIdentity(new[] { 
                        //indica o tipo da informação da entidade
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.GivenName, user.nome_User),
                        new Claim(ClaimTypes.Role, user.tipo.ToString()),
                        //Adicionando o + " " depois do int, ele automaticamente vira uma string
                        new Claim(ClaimTypes.Sid, user.UserID + ""),
                        //Aparentemente este é necessário para que o programa de problema com o TokenValidation
                        new Claim(ClaimTypes.NameIdentifier, user.UserID + "")
                    }, "ApplicationCookie");

                    //Acessando o Contexto Owin
                    var context = Request.GetOwinContext();
                    //Chama o gerenciador de autenticação
                    var authManager = context.Authentication;
                    //Adiciona a entidade criada
                    authManager.SignIn(identity);

                    /*var userInfo = db.Usuario.Find(user.UserID); 
                    ViewBag.user_info = userInfo;*/

                    ViewBag.name_user = user.nome_User;

                    bool continuar_cadastro = false;

                    if (user.sobrenome_User.Equals("xx") || user.sobrenome_User == null)
                    {
                        continuar_cadastro = true;
                    }
                    if (user.telefone_User.Equals("xx"))
                    {
                        continuar_cadastro = true;
                    }
                    /*if (user.data_nascimento.Equals("xx"))
                    {
                        continuar_cadastro = true;
                    }*/

                    //CAMPO REMOVIDO:
                    //user.data_nascimento.Equals("xx") || user.data_nascimento == null ||

                    if (user.nome_User.Equals("xx") || user.nome_User == null || user.cpf_User.Equals("xx") || user.cpf_User == null ||
                    user.telefone_User.Equals("xx") || user.telefone_User == null ||
                    user.Rua.Equals("xx") || user.Rua == null || user.Bairro.Equals("xx") || user.Bairro == null || user.Cidade.Equals("xx") ||
                    user.Cidade == null || user.Estado.Equals("xx") || user.Estado == null)
                    {
                        continuar_cadastro = true;
                    }

                    var to_cadastro = new UsuarioModelCadastro
                    {
                        nome_usuario = user.nome_User,
                        sobrenome_usuario = user.sobrenome_User,
                        cpf_usuario = user.cpf_User,
                        telefone_usuario = user.telefone_User,
                        //data_nascimento_usuario = user.data_nascimento,
                        email_usuario = user.email_User,
                        senha = " ",
                        confirmar_senha = " ",
                        Rua = user.Rua,
                        Bairro = user.Bairro,
                        Cidade = user.Cidade,
                        Estado = user.Estado
                    };

                    if (continuar_cadastro)
                    {
                        ViewBag.completar = to_cadastro;
                        return View("Cadastro", to_cadastro);
                    }
                    else
                    {
                        return RedirectToAction("Principal");
                    }

                }
                else
                {
                    //O primeiro parâmetro é o nome do input onde esse erro vai ficar. No caso esse erro será colocado embaixo
                    //do input que recebe o model.Email
                    ModelState.AddModelError("Email", "Nome de usuário ou senha incorretos");
                    return View("Index", usuario);
                }
            }
            else
            {
                //Aqui não precisa de addModelError pq já está no [Required]
                return View("Index", usuario);
            }

            
        }

        public ActionResult LogOut()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignOut("ApplicationCookie");
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Principal()
        {
            var db = new ToolsContext();

            var ferramentas = db.Ferramenta.Where(l => l.FerramentaID >= 1).ToList();

            ViewBag.Ferramentas = ferramentas;

            return View();
        }

        public ActionResult Perfil()
        {
            //Quando o item retornado é igual ao modelo que está na view ele automaticamente preenche os dados caso seja apenas um item
            //Senão deve ser usado o Foreach

            var userAutenticado = (ClaimsIdentity)User.Identity;

            var db = new ToolsContext();

            int id = Convert.ToInt32(userAutenticado.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

            var userEncontrado = db.Usuario.Find(id);

            var usuario = new UsuarioModelCadastro
            {
                nome_usuario = userEncontrado.nome_User,
                sobrenome_usuario = userEncontrado.sobrenome_User,
                cpf_usuario = userEncontrado.cpf_User,
                //data_nascimento_usuario = userEncontrado.data_nascimento,
                email_usuario = userEncontrado.email_User,
                telefone_usuario = userEncontrado.telefone_User,
                senha = "",
                confirmar_senha = "",
                Rua = userEncontrado.Rua,
                Bairro = userEncontrado.Bairro,
                Cidade = userEncontrado.Cidade,
                Estado = userEncontrado.Estado,
                numero_casa = userEncontrado.num_casa,
                complemento_casa = Convert.ToString(userEncontrado.complemento)
            };

            var minhas_Ferramentas = db.Ferramenta.Where(item => item.VendedorID == id).ToList();

            ViewBag.minhas_ferramentas = minhas_Ferramentas;

            var minhas_compras_anteriores = db.Compras.Where(compra => compra.clienteID == id).ToList();

            ViewBag.minhas_compras_anteriores = minhas_compras_anteriores;

            //CAMPO REMOVIDO:
            //userEncontrado.data_nascimento.Equals("xx") || userEncontrado.data_nascimento == null || 

            //Ou checar se é igual a Null
            if (userEncontrado.nome_User.Equals("xx") || userEncontrado.nome_User == null || userEncontrado.cpf_User.Equals("xx") || userEncontrado.cpf_User == null ||
                userEncontrado.telefone_User.Equals("xx") || userEncontrado.telefone_User == null || 
                userEncontrado.Rua.Equals("xx") || userEncontrado.Rua == null || userEncontrado.Bairro.Equals("xx") || userEncontrado.Bairro == null || userEncontrado.Cidade.Equals("xx") ||
                userEncontrado.Cidade == null || userEncontrado.Estado.Equals("xx") || userEncontrado.Estado == null)
            {
                ViewBag.cadastro_completo = false;
            }
            else
            {
                ViewBag.cadastro_completo = true;
            }

            return View(usuario);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Pesquisar(string pesquisa)
        {
            //Se eu colocar no parametro o mesmo nome do elemento html, ele vai receber o valor daquele input
            var db = new ToolsContext();

            var ferramenta = db.Ferramenta.Where(l => l.NomeFerramenta.ToLower().Contains(pesquisa.ToLower())).ToList();

            ViewBag.ferramenta_pesquisada = ferramenta;

            return View("Principal");
        }

        [AllowAnonymous]
        public ActionResult Cadastro()
        {
            //Ajustar para redirecionar
            return View();
        }

        [AllowAnonymous]
        public ActionResult CadastrarUsuario(UsuarioModelCadastro user, string redirect_ = null, int ferramentaID_ = 0)
        {
            if (ModelState.IsValid)
            {
                if (user.senha.Equals(user.confirmar_senha))
                {
                    var db = new ToolsContext();

                    bool continuar_cadastro = false;

                    var usuario = new Usuario
                    {
                        UserID = 2,
                        nome_User = user.nome_usuario,
                        sobrenome_User = user.sobrenome_usuario,
                        cpf_User = user.cpf_usuario,
                        email_User = user.email_usuario,
                        senha = Usuario.GerarHash(user.senha),
                        //data_nascimento = user.data_nascimento_usuario,
                        telefone_User = user.telefone_usuario,
                        Rua = user.Rua,
                        Bairro = user.Bairro,
                        Cidade = user.Cidade,
                        Estado = user.Estado,
                        tipo = TipoDeUsuario.CLIENTE,
                        num_casa =  user.numero_casa,
                        complemento = Convert.ToInt32(user.complemento_casa)
                    };

                    if(user.ImagemPerfil != null)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(user.ImagemPerfil.InputStream))
                        {
                            usuario.ImagemPerfil = binaryReader.ReadBytes(user.ImagemPerfil.ContentLength);
                        }
                        
                    }

                    if(user.sobrenome_usuario == null)
                    {
                        usuario.sobrenome_User = "xx";
                        continuar_cadastro = true;
                    }
                    /*if(user.data_nascimento_usuario == null)
                    {
                        usuario.data_nascimento = "xx";
                        continuar_cadastro = true;
                    }*/
                    if(user.telefone_usuario == null)
                    {
                        usuario.telefone_User = "xx";
                        continuar_cadastro = true;
                    }

                    db.Usuario.Add(usuario);
                    db.SaveChanges();

                    var userAtual = db.Usuario.Find(usuario.UserID);

                    var identity = new ClaimsIdentity(new[] 
                    { 
                        new Claim(ClaimTypes.Email, userAtual.email_User),
                        new Claim(ClaimTypes.GivenName, userAtual.nome_User),
                        new Claim(ClaimTypes.Role, userAtual.tipo.ToString()),
                        new Claim(ClaimTypes.Sid, userAtual.UserID.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userAtual.UserID.ToString()),

                    }, "ApplicationCookie");

                    //Acessando o Contexto Owin
                    var context = Request.GetOwinContext();
                    //Chama o gerenciador de autenticação
                    var authManager = context.Authentication;
                    //Adiciona a entidade criada
                    authManager.SignIn(identity);

                    /*var userInfo = db.Usuario.Find(user.UserID); 
                    ViewBag.user_info = userInfo;*/

                    ViewBag.name_user = userAtual.nome_User;

                    if (continuar_cadastro)
                    {
                        ViewBag.completar = usuario;
                        ViewBag.Redirect = redirect_;
                        if(ferramentaID_ > 0)
                        {
                            var ja_existe = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramentaID_ && l.ClienteID == usuario.UserID)).FirstOrDefault();
                            if (ja_existe == null)
                            {
                                var adicionar_ao_carrinho = new MeuCarrinho
                                {
                                    FerramentaID = ferramentaID_,
                                    ClienteID = usuario.UserID

                                };
                                db.MeuCarrinho.Add(adicionar_ao_carrinho);
                                db.SaveChanges();
                            }
                        }
                        return View("Cadastro");
                    }
                    else
                    {
                        if(redirect_ == null)
                        {
                            return RedirectToAction("Principal");
                        }
                        else
                        {
                            var ja_existe = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramentaID_ && l.ClienteID == usuario.UserID)).FirstOrDefault();
                            if (ja_existe == null)
                            {
                                var adicionar_ao_carrinho = new MeuCarrinho
                                {
                                    FerramentaID = ferramentaID_,
                                    ClienteID = usuario.UserID

                                };
                                db.MeuCarrinho.Add(adicionar_ao_carrinho);
                                db.SaveChanges();
                            }
                            return RedirectToAction(redirect_);
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("senha", "Senhas não estão iguais");
                    ModelState.AddModelError("confirmar_senha", "Senhas não estão iguais");
                    return View("Cadastro", user);
                }
            }
            else
            {
                //var teste = ModelState.Values;
                return View("Cadastro", user);
            }
        }

        public ActionResult CompletarCadastro(UsuarioModelCadastro user)
        {
            var usuarioAutenticado = (ClaimsIdentity)User.Identity;
            var db = new ToolsContext();

            if (usuarioAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(usuarioAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var usuario = db.Usuario.Find(id);

                if (user.nome_usuario != null) { usuario.nome_User = user.nome_usuario; }
                if (user.sobrenome_usuario != null) { usuario.sobrenome_User = user.sobrenome_usuario; }
                if (user.telefone_usuario != null) { usuario.telefone_User = user.telefone_usuario; }
                //if(user.data_nascimento_usuario != null) { usuario.data_nascimento = user.data_nascimento_usuario; }
                if(user.cpf_usuario != null) { usuario.cpf_User = user.cpf_usuario; }
                if (user.Rua != null) { usuario.Rua = user.Rua; }
                if (user.Bairro != null) { usuario.Bairro = user.Bairro; }
                if (user.Cidade != null) { usuario.Cidade = user.Cidade; }
                if (user.Estado != null) { usuario.Estado = user.Estado; }

                db.SaveChanges();

                //Remover a conta antiga para atualizar os dados
                usuarioAutenticado.RemoveClaim(usuarioAutenticado.Claims.Where(c => c.Type == ClaimTypes.GivenName).FirstOrDefault());
                //Inserir os novos dados
                usuarioAutenticado.AddClaim(new Claim(ClaimTypes.GivenName, user.nome_usuario));

            }
            
            return RedirectToAction("Principal");
        }

        [ValidateAntiForgeryToken]
        public ActionResult CadastrarADM(string Email, string Senha, string CSenha)
        {

            if (Senha.Equals(CSenha))
            {
                var db = new ToolsContext();

                var usuario = new Usuario
                {
                    UserID = 2,
                    nome_User = Email,
                    sobrenome_User = "xx",
                    cpf_User = "xx",
                    email_User = Email,
                    telefone_User = "xx",
                    senha = Usuario.GerarHash(Senha),
                    Rua = "xx",
                    Bairro = "xx",
                    Cidade = "xx",
                    Estado = "xx",
                    tipo = TipoDeUsuario.ADMINISTRADOR
                };

                db.Usuario.Add(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarInfos(UsuarioModelCadastro userModel)
        {
            var usuarioAutenticado = (ClaimsIdentity)User.Identity;
            var db = new ToolsContext();
            Usuario usuario;

            if (usuarioAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(usuarioAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                usuario = db.Usuario.Find(id);

                if (userModel.nome_usuario != null) { usuario.nome_User = userModel.nome_usuario; }
                if (userModel.sobrenome_usuario != null) { usuario.sobrenome_User = userModel.sobrenome_usuario; }
                if(userModel.email_usuario != null) { usuario.email_User = userModel.email_usuario; }
                if(userModel.telefone_usuario != null) { usuario.telefone_User = userModel.telefone_usuario; }
                if(userModel.senha != null && userModel.confirmar_senha != null) 
                {
                    if (userModel.senha.Equals(userModel.confirmar_senha))
                    {
                        usuario.senha = Usuario.GerarHash(userModel.senha);
                    }
                }
                if(userModel.Rua != null) { usuario.Rua = userModel.Rua; }
                if(userModel.Bairro != null) { usuario.Bairro = userModel.Bairro; }
                if(userModel.Cidade != null) { usuario.Cidade = userModel.Cidade; }
                if(userModel.Estado != null) { usuario.Estado = userModel.Estado; }
                if (userModel.ImagemPerfil != null)
                {
                    using (BinaryReader binaryReader = new BinaryReader(userModel.ImagemPerfil.InputStream))
                    {
                        usuario.ImagemPerfil = binaryReader.ReadBytes(userModel.ImagemPerfil.ContentLength);
                    }

                }
                if(userModel.numero_casa > 0) { usuario.num_casa = userModel.numero_casa; }
                if(userModel.complemento_casa != null) { usuario.complemento = Convert.ToInt32(userModel.complemento_casa); }

                db.SaveChanges();

                //Remover a conta antiga para atualizar os dados
                usuarioAutenticado.RemoveClaim(usuarioAutenticado.Claims.Where(c => c.Type == ClaimTypes.GivenName).FirstOrDefault());
                //Inserir os novos dados
                usuarioAutenticado.AddClaim(new Claim(ClaimTypes.GivenName, userModel.nome_usuario));
            }

            return View("Perfil", userModel);
        }

        public ActionResult MinhasFerramentas()
        {
            var usuarioAutenticado = (ClaimsIdentity)User.Identity;
            var db = new ToolsContext();

            int id = Convert.ToInt32(usuarioAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

            var minhas_Ferramentas = db.Ferramenta.Where(item => item.VendedorID == id).ToList();

            ViewBag.minhas_ferramentas = minhas_Ferramentas;

            return View();
        }

        [HttpPost]
        public ActionResult AdicionarFerramenta(FerramentaCadastro ferramenta)
        {
            if (ModelState.IsValid)
            {
                var db = new ToolsContext();
                var userAutenticado = (ClaimsIdentity)User.Identity;
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var ferramenta_cadastrada = new Ferramenta
                {
                    NomeFerramenta = ferramenta.NomeFerramenta,
                    ValorFerramenta = ferramenta.ValorFerramenta,
                    VendedorID = id,
                    modelo_ferramenta = ferramenta.modelo_ferramenta,
                    marca_ferramenta = ferramenta.marca_ferramenta,
                    cod_nacional = ferramenta.cod_nacional,
                    disponibilidade = Disponibilidade.DISPONIVEL,
                    utilizado = Condition.NOVO,
                    quantidade = ferramenta.quantidade
                };

                if (ferramenta.ImagemFerramenta != null)
                {
                    using (BinaryReader binaryReader = new BinaryReader(ferramenta.ImagemFerramenta.InputStream))
                    {
                        ferramenta_cadastrada.ImagemFerramenta = binaryReader.ReadBytes(ferramenta.ImagemFerramenta.ContentLength);
                    }

                }

                db.Ferramenta.Add(ferramenta_cadastrada);
                db.SaveChanges();

                var suas_ferramentas = db.Ferramenta.Where(l => l.VendedorID == id).ToList();
                ViewBag.minhas_ferramentas = suas_ferramentas;

                ModelState.Clear();

                return View("MinhasFerramentas");
            }
            else
            {
                return View("MinhasFerramentas", ferramenta);
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult AlterarFerramenta(FerramentaAlterar ferramenta_to_change)
        {
            if (ModelState.IsValid)
            {
                var db = new ToolsContext();
                var userAutenticado = (ClaimsIdentity)User.Identity;
                Ferramenta ferramenta;
                if (userAutenticado.IsAuthenticated)
                {
                    int id = Convert.ToInt32(userAutenticado.Claims.Where
                        (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);
                
                    ferramenta = db.Ferramenta.Where(l => (l.NomeFerramenta.Equals(ferramenta_to_change.NomeAnterior) && l.VendedorID == id)).FirstOrDefault();
                
                    ferramenta.NomeFerramenta = ferramenta_to_change.NomeFerramenta;
                    ferramenta.ValorFerramenta = ferramenta_to_change.ValorFerramenta;
                    ferramenta.modelo_ferramenta = ferramenta_to_change.modelo_ferramenta;
                    ferramenta.marca_ferramenta = ferramenta_to_change.marca_ferramenta;
                    ferramenta.cod_nacional = ferramenta_to_change.cod_nacional;
                    ferramenta.quantidade = ferramenta_to_change.quantidade;

                    if (ferramenta_to_change.utilizado.ToString().Equals("NOVO"))
                    {
                        ferramenta.utilizado = Condition.NOVO;
                    }
                    else
                    {
                        ferramenta.utilizado = Condition.USADO;
                    }

                    if (ferramenta_to_change.ImagemFerramenta != null)
                    {
                        using (BinaryReader binaryReader = new BinaryReader(ferramenta_to_change.ImagemFerramenta.InputStream))
                        {
                            ferramenta.ImagemFerramenta = binaryReader.ReadBytes(ferramenta_to_change.ImagemFerramenta.ContentLength);
                        }

                    }

                    var minhas_Ferramentas = db.Ferramenta.Where(item => item.VendedorID == id).ToList();

                    ViewBag.minhas_ferramentas = minhas_Ferramentas;
                
                    db.SaveChanges();
                    return RedirectToAction("Item", new { ferramentaID = ferramenta.FerramentaID});
                }
                else
                {
                    return View("MinhasFerramentas");
                }
            }
            else
            {
                return View("MinhasFerramentas");
            }


        }

        [AllowAnonymous]
        public ActionResult Item(int ferramentaID)
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var usuario = db.Usuario.Find(id);
                ViewBag.User_atual = usuario;
            }

            var ferramenta_to_change = db.Ferramenta.Where(l => (l.FerramentaID == ferramentaID)).FirstOrDefault();
            var vendedor = db.Usuario.Find(ferramenta_to_change.VendedorID);

            ViewBag.Esta_Ferramenta = ferramenta_to_change;
            ViewBag.Vendedor = vendedor;

            var ferramenta_passar = new FerramentaAlterar
            {
                NomeAnterior = ferramenta_to_change.NomeFerramenta,
                NomeFerramenta = ferramenta_to_change.NomeFerramenta,
                ValorFerramenta = ferramenta_to_change.ValorFerramenta,
                modelo_ferramenta = ferramenta_to_change.modelo_ferramenta,
                marca_ferramenta = ferramenta_to_change.marca_ferramenta,
                cod_nacional = Convert.ToInt32(ferramenta_to_change.cod_nacional),
                quantidade = ferramenta_to_change.quantidade
            };

            if (ferramenta_to_change.utilizado == Condition.NOVO)
            {
                ferramenta_passar.utilizado = Condition.NOVO;
            }
            else
            {
                ferramenta_passar.utilizado = Condition.USADO;
            }

            return View(ferramenta_passar);
        }

        public ActionResult ApagarConta()
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var usuario = db.Usuario.Find(id);

                var ferramentas = db.Ferramenta.Where(item => item.VendedorID == id).ToList();

                foreach(var item in ferramentas)
                {
                    db.Ferramenta.Remove(item);
                }

                db.Usuario.Remove(usuario);
                db.SaveChanges();
            }
            return RedirectToAction("LogOut");
        }

        public ActionResult ApagarFerramenta(int ferramentaID)
        {
            var db = new ToolsContext();

            var ferramenta_apagar = db.Ferramenta.Find(ferramentaID);

            db.Ferramenta.Remove(ferramenta_apagar);
            db.SaveChanges();

            return RedirectToAction("MinhasFerramentas");
        }

        [AllowAnonymous]
        public ActionResult Carrinho()
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var ferramentas_carrinho = db.MeuCarrinho.Where(l => l.ClienteID == id).ToList();

                List<double> quantidades = new List<double>();

                List<Ferramenta> ferramentas = new List<Ferramenta>();

                foreach(var item in ferramentas_carrinho)
                {
                    var x = db.Ferramenta.Find(item.FerramentaID);
                    quantidades.Add(Convert.ToDouble(item.Quantidade));
                    if (x != null)
                    {
                        ferramentas.Add(x);
                    }
                }

                ViewBag.MeuCarrinho = ferramentas;
                ViewBag.Quantidades = quantidades;
            }
            else
            {
                ViewBag.MeuCarrinho = null;
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdicionarAoCarrinho(int ferramentaID_, int Quantidade_)
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var ja_existe = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramentaID_&& l.ClienteID == id)).FirstOrDefault();
                if(ja_existe == null)
                {
                    var adicionar_ao_carrinho = new MeuCarrinho
                    {
                        FerramentaID = ferramentaID_,
                        ClienteID = id
                    };
                    if(Quantidade_ > 0)
                    {
                        adicionar_ao_carrinho.Quantidade = Quantidade_;
                    }

                    db.MeuCarrinho.Add(adicionar_ao_carrinho);
                    db.SaveChanges();
                }
                return RedirectToAction("Item", new { ferramentaID = ferramentaID_});
            }
            else
            {
                ViewBag.Redirect = "Carrinho";
                ViewBag.Ferramenta_ID = ferramentaID_;
                return View("Cadastro");
            }

        }

        [AllowAnonymous]
        public ActionResult RemoverDoCarrinho(int ferramentaID_)
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var ferramenta_remover = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramentaID_ || l.ClienteID == id)).FirstOrDefault();

                db.MeuCarrinho.Remove(ferramenta_remover);
                db.SaveChanges();

            }

            return RedirectToAction("Carrinho");
        }

        [ValidateAntiForgeryToken]
        public ActionResult FinalizarCompra(double valor_compra, CompraModel compra_)
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            DateTime dataHoje = DateTime.Now;
            var id_pedido = new Random().Next(1, 9999);

            if (ModelState.IsValid)
            {
                if (userAutenticado.IsAuthenticated)
                {
                    if(compra_.num_cartao == null || compra_.nome_cartao == null || compra_.validade_cartao == null)
                    {
                        ModelState.AddModelError("num_cartao", "Informações do cartão são obrigatórias nesta forma de pagamento");
                        return RedirectToAction("Carrinho", compra_);
                    }
                    else
                    {
                        int id = Convert.ToInt32(userAutenticado.Claims.Where
                        (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);
                        var ferramentas_compra = db.MeuCarrinho.Where(l => l.ClienteID == id);

                        var localiza_existe = db.Compras.Find(id_pedido);
                        while (localiza_existe != null)
                        {
                            id_pedido += 1;

                            localiza_existe = db.Compras.Find(id_pedido);
                        }

                        foreach (MeuCarrinho item in ferramentas_compra)
                        {
                            //Remove do estoque
                            var ferramenta = db.Ferramenta.Where(l => l.FerramentaID == item.FerramentaID).FirstOrDefault();

                            var pedido_ferramenta = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramenta.FerramentaID && l.ClienteID == id)).FirstOrDefault();

                            //Adiciona ao pedido e a compra

                            var quantidade = ferramenta.quantidade - pedido_ferramenta.Quantidade;


                            /*Pedido pedido = new Pedido
                            {
                                numero_pedido = compra_cod.Count + 1,
                                valor_compra = valor_compra,
                                ferramentaID = ferramenta.FerramentaID,
                                VendedorID = ferramenta.VendedorID,
                                CompradorID = id,
                                pedido_venda = compra_cod.Count + 1,
                                quantidade_ferramenta = pedido_ferramenta.Quantidade,
                                data_compra = dataHoje.ToString()
                            };*/

                            Compras compra = new Compras
                            {
                                pedidosID = id_pedido,
                                clienteID = id,
                                valorCompra = valor_compra,
                                num_Cartao = compra_.num_cartao,
                                nome_Cartao = compra_.nome_cartao,
                                validade_Cartao = compra_.validade_cartao,
                                parcelas_Compra = compra_.parcelas_compra,
                                Data_Compra = dataHoje.ToString(),
                                VendedorID = ferramenta.VendedorID,
                                pedido_venda = id_pedido,
                                quantidade_ferramenta = pedido_ferramenta.Quantidade,
                                ferramentaID = ferramenta.FerramentaID,
                                forma_pagamento = Formas_Pagamento.CREDITO
                            };

                            ferramenta.quantidade = quantidade;

                            //db.Pedido.Add(pedido);
                            db.Compras.Add(compra);

                            db.MeuCarrinho.Remove(pedido_ferramenta);
                        }

                        //Adicionar ainda informaações do cartão e informações do cliente

                        db.SaveChanges();
                        return RedirectToAction("Principal");
                    }
                }
                else
                {
                    return RedirectToAction("Carrinho", compra_);
                }
            }
            else
            {
                return RedirectToAction("Carrinho", compra_);
            }
        }

        public ActionResult Outras_formas_pagamento(double valor_compra, int forma_pag)
        {
            var db = new ToolsContext();
            var userAutenticado = (ClaimsIdentity)User.Identity;
            DateTime dataHoje = DateTime.Now;
            var id_pedido = new Random().Next(1, 9999);

            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);
                var ferramentas_compra = db.MeuCarrinho.Where(l => l.ClienteID == id);

                var localiza_existe = db.Compras.Find(id_pedido);
                while (localiza_existe != null)
                {
                    id_pedido += 1;

                    localiza_existe = db.Compras.Find(id_pedido);
                }

                foreach (MeuCarrinho item in ferramentas_compra)
                {
                    //Remove do estoque
                    var ferramenta = db.Ferramenta.Where(l => l.FerramentaID == item.FerramentaID).FirstOrDefault();

                    var pedido_ferramenta = db.MeuCarrinho.Where(l => (l.FerramentaID == ferramenta.FerramentaID && l.ClienteID == id)).FirstOrDefault();

                    //Adiciona ao pedido e a compra

                    var quantidade = ferramenta.quantidade - pedido_ferramenta.Quantidade;


                    Compras compra = new Compras
                    {
                        pedidosID = id_pedido,
                        clienteID = id,
                        valorCompra = valor_compra,
                        parcelas_Compra = 1,
                        Data_Compra = dataHoje.ToString(),
                        VendedorID = ferramenta.VendedorID,
                        pedido_venda = id_pedido,
                        quantidade_ferramenta = pedido_ferramenta.Quantidade,
                        ferramentaID = ferramenta.FerramentaID
                    };

                    if(forma_pag == 1)
                    {
                        compra.forma_pagamento = Formas_Pagamento.BOLETO;
                    }
                    else
                    {
                        compra.forma_pagamento = Formas_Pagamento.PIX;
                    }

                    ferramenta.quantidade = quantidade;

                    //db.Pedido.Add(pedido);
                    db.Compras.Add(compra);

                    db.MeuCarrinho.Remove(pedido_ferramenta);
                }

                //Adicionar ainda informaações do cartão e informações do cliente

                db.SaveChanges();
                return RedirectToAction("Principal");
            }
            else
            {
                return RedirectToAction("Carrinho");
            }
        }

        [AllowAnonymous]
        public ActionResult Perfil_Cliente(int cliente_id)
        {
            var db = new ToolsContext();

            var userAutenticado = (ClaimsIdentity)User.Identity;

            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                        (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                if(id != cliente_id) 
                {
                    var cliente_ = db.Usuario.Find(cliente_id);

                    ViewBag.cliente_ = cliente_;

                    var ferramentas = db.Ferramenta.Where(l => l.VendedorID == cliente_id).ToList();

                    ViewBag.ferramentas_cliente = ferramentas;

                    return View();
                }
                else
                {
                    return RedirectToAction("Perfil");
                }
            }
            else
            {
                var cliente_ = db.Usuario.Find(cliente_id);

                ViewBag.cliente_ = cliente_;

                var ferramentas = db.Ferramenta.Where(l => l.VendedorID == cliente_id).ToList();

                ViewBag.ferramentas_cliente = ferramentas;

                return View();
            }

        }

        [AllowAnonymous]
        public ActionResult Pedidos()
        {
            var db = new ToolsContext();
            var usuarioAutenticado = (ClaimsIdentity)User.Identity;

            if (usuarioAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(usuarioAutenticado.Claims.Where
                    (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var compras = db.Compras.Where(l => l.VendedorID == id).ToList();

                List<ModeloPedidos> pedidos_total = new List<ModeloPedidos>();

                foreach(Compras item in compras)
                {
                    var cliente = db.Usuario.Find(item.clienteID);

                    var ferramenta_comprada = db.Ferramenta.Find(item.ferramentaID);

                    var compra = db.Compras.Where(l => (l.pedidosID == item.pedidosID && ferramenta_comprada.FerramentaID == item.ferramentaID && item.VendedorID == id)).FirstOrDefault();

                    var encontrado = new ModeloPedidos
                    {
                        numero_pedido = item.pedidosID,
                        nome_comprador = cliente.nome_User,
                        nome_ferramenta = ferramenta_comprada.NomeFerramenta,
                        quantidade_ferramenta = item.quantidade_ferramenta,
                        valor_compra = item.valorCompra,
                        parcelas = compra.parcelas_Compra
                    };

                    if(compra.forma_pagamento == Formas_Pagamento.BOLETO)
                    {
                        encontrado.forma_pagamento = "BOLETO";
                    }
                    else if(compra.forma_pagamento == Formas_Pagamento.CREDITO)
                    {
                        encontrado.forma_pagamento = "CARTÃO DE CRÉDITO";
                    }
                    else
                    {
                        encontrado.forma_pagamento = "PIX";
                    }

                    pedidos_total.Add(encontrado);
                }

                ViewBag.ModeloPedidos = pedidos_total;

            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Suporte()
        {
            var db = new ToolsContext();

            var userAutenticado = (ClaimsIdentity)User.Identity;

            if (userAutenticado.IsAuthenticated)
            {
                int id = Convert.ToInt32(userAutenticado.Claims.Where
                        (c => c.Type == ClaimTypes.Sid).FirstOrDefault().Value);

                var cliente_ = db.Usuario.Find(id);

                ViewBag.cliente = cliente_;
            }

            return View();
        }   

        [HttpPost]
        public ActionResult EnviarEmail(string mensagem, string Assunto_msg)
        {
            EnviarMensagem.enviar_Email(Assunto_msg, mensagem);

            return RedirectToAction("Suporte");

        }

        [AllowAnonymous]
        public ActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RSenha(AlterarSenhaModel dados)
        {
            if(dados.Email != null)
            {
                var cod_redef = new Random().Next(10000, 99999);
                EnviarMensagem.enviar_Email("Redefinir Senha", "Olá, o código de segurança para redefinir sua senha é: <br /><h1 style=\"text-align:center;\">" + cod_redef + "</h1>", dados.Email);

                ViewBag.OpenInserir = true;
                dados.CodEnviado = Usuario.GerarHash(cod_redef.ToString());
                return View("EsqueciSenha", dados);
            }
            else
            {
                ModelState.AddModelError("Email", "O campo E-mail é obrigatório!");
                return View("EsqueciSenha", dados);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RSenhaW(AlterarSenhaModel dados)
        {
            if(dados.EmailWpp != null && dados.NumeroWpp != null)
            {
                var cod_redef = new Random().Next(10000, 99999);
                //Redirect("https://wa.me/5511959786924?text="+ "Olá, o código de segurança para redefinir sua senha é: " + cod_redef);
                Response.Write("<script>" +
                    "var win = window.open('https://wa.me/55"+dados.NumeroWpp+"?text=Olá, o código de segurança para redefinir sua senha é: '+"+ cod_redef + ", '_blank');" +
                    "win.focus();" +
                    "</script>");
                ViewBag.OpenInserir = true;
                dados.CodEnviado = Usuario.GerarHash(cod_redef.ToString());
                return View("EsqueciSenha", dados);
            }
            else
            {
                if(dados.EmailWpp == null)
                {
                    //Erro Email
                    ModelState.AddModelError("EmailWpp", "Email é obrigatório");
                    return View("EsqueciSenha", dados);
                }
                if(dados.NumeroWpp == null)
                {
                    //Erro Numero
                    ModelState.AddModelError("NumeroWpp", "Número é obrigatório");
                    return View("EsqueciSenha", dados);
                }
                else
                {
                    return View("EsqueciSenha", dados);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CompararCod(AlterarSenhaModel dados)
        {
            if(dados.CodInserido != null)
            {
                if (Usuario.GerarHash(dados.CodInserido).Equals(dados.CodEnviado))
                {
                    ViewBag.Metho = true;
                    return View("EsqueciSenha", dados);
                }
                else
                {
                    ViewBag.OpenInserir = true;
                    ModelState.AddModelError("CodInserido", "Código Incorreto!");
                    return View("EsqueciSenha", dados);
                }
            }
            else
            {
                ViewBag.OpenInserir = true;
                ModelState.AddModelError("CodInserido", "Código é obrigatório!");
                return View("EsqueciSenha", dados);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EnviarNovaSenha(AlterarSenhaModel dados)
        {
            var db = new ToolsContext();
            Usuario usuario_alterar;

            if (dados.Email != null)
            {
                if (dados.SenhaNova == null || dados.ConfirmarSenhaNova == null)
                {
                    if (dados.SenhaNova == null)
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("SenhaNova", "Este campo é obrigatório");
                        return View("EsqueciSenha", dados);
                    }
                    if (dados.ConfirmarSenhaNova == null)
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("ConfirmarSenhaNova", "Este campo é obrigatório");
                        return View("EsqueciSenha", dados);
                    }
                    else
                    {
                        ViewBag.Metho = true;
                        return View("EsqueciSenha", dados);
                    }
                }
                else
                {
                    if (dados.SenhaNova.Equals(dados.ConfirmarSenhaNova))
                    {
                        usuario_alterar = db.Usuario.Where(dado => dado.email_User == dados.EmailWpp).FirstOrDefault();

                        usuario_alterar.senha = Usuario.GerarHash(dados.SenhaNova);

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("SenhaNova", "Senhas não estão iguais");
                        ModelState.AddModelError("ConfirmarSenhaNova", "Senhas não estão iguais");
                        return View("EsqueciSenha", dados);
                    }
                }
            }
            else if (dados.EmailWpp != null)
            {
                if(dados.SenhaNova == null || dados.ConfirmarSenhaNova == null)
                {
                    if(dados.SenhaNova == null)
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("SenhaNova", "Este campo é obrigatório");
                        return View("EsqueciSenha", dados);
                    }
                    if (dados.ConfirmarSenhaNova == null)
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("ConfirmarSenhaNova", "Este campo é obrigatório");
                        return View("EsqueciSenha", dados);
                    }
                    else
                    {
                        ViewBag.Metho = true;
                        return View("EsqueciSenha", dados);
                    }
                }
                else
                {
                    if (dados.SenhaNova.Equals(dados.ConfirmarSenhaNova))
                    {
                        usuario_alterar = db.Usuario.Where(dado => dado.email_User == dados.EmailWpp).FirstOrDefault();

                        usuario_alterar.senha = Usuario.GerarHash(dados.SenhaNova);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Metho = true;
                        ModelState.AddModelError("SenhaNova", "Senhas não estão iguais");
                        ModelState.AddModelError("ConfirmarSenhaNova", "Senhas não estão iguais");
                        return View("EsqueciSenha", dados);
                    }
                }
            }
            else
            {
                ViewBag.Metho = true;
                return View("EsqueciSenha", dados);
            }
        }
    }
}