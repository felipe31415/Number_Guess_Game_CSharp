using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace _JogoFefe
{
    class Program
    {
        public static void Main(string[] args)
        {
            string nome;
            string email;
            string senha;
            int op;
            Login login = new Login();
            List<Cadastro> cadastros = new List<Cadastro>();

            while (true)
            {
                try
                {
                    Console.WriteLine("Escolha uma ação: ");
                    Console.WriteLine("\n 1. Efetuar Cadastro\n 2. Efetuar Login\n 3. Jogar jogo\n 4. Mostrar Estatísticas\n 5. Ver Placar\n 6. Sair");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:

                            Console.WriteLine("Digite o nome: ");
                            nome = Console.ReadLine();

                            Console.WriteLine("Digite o seu e-mail: ");
                            email = Console.ReadLine();

                            Console.WriteLine("Digite uma senha: ");
                            senha = Console.ReadLine();

                            Cadastro novoCadastro = new Cadastro(nome, email, senha);
                            cadastros.Add(novoCadastro);
                            break;

                        case 2:

                            while (true)
                            {
                                Console.WriteLine("Digite o seu e-mail: ");
                                email = Console.ReadLine();

                                Console.WriteLine("Digite a sua senha: ");
                                senha = Console.ReadLine();

                                login.Email = email;
                                login.Password = senha;

                                login.Cadastro = cadastros.FirstOrDefault(c => c.Email.Equals(login.Email, StringComparison.OrdinalIgnoreCase));

                                if (login.Cadastro != null && login.Cadastro.Password == login.Password)
                                {
                                    Console.WriteLine($"O login foi efetuado com sucesso!\n\n Nome: {login.Cadastro.Name}\n E-mail: {login.Cadastro.Email}\n ID: {login.Cadastro.Id}");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Seu e-mail ou senha estão errados!");
                                    Console.WriteLine("Tente novamente: ");
                                    continue;
                                }

                            }

                            break;

                        case 3:

                            while (true)
                            {
                                Console.WriteLine("Você deseja: \n\n 1.Começar novo jogo\n 2.Continuar jogo existente\n 3.Sair");
                                op = int.Parse(Console.ReadLine());

                                if (op == 3)
                                {
                                    break;
                                }

                                if (op == 1)
                                {
                                    Console.WriteLine("Digite o nome do jogo: ");
                                    nome = Console.ReadLine();

                                    if (login.Cadastro.Games.FirstOrDefault(g => g.Name.Equals(nome, StringComparison.OrdinalIgnoreCase)) != null)
                                    {
                                        Console.WriteLine("Nome já existente!");
                                        continue;
                                    }

                                    Game game = new Game(nome);

                                    login.Cadastro.Games.Add(game);

                                    game.Play();
                                    continue;
                                }

                                Console.WriteLine("Digite o nome do jogo: ");
                                nome = Console.ReadLine();

                                var jogoAtual = login.Cadastro.Games.FirstOrDefault(g => g.Name.Equals(nome, StringComparison.OrdinalIgnoreCase));

                                if (jogoAtual == null)
                                {
                                    Console.WriteLine("Nome inválido!");
                                    continue;
                                }

                                jogoAtual.Play();
                            }

                            break;
                        case 4:
                            Console.WriteLine("Id\t\t Nome\t\t Nível\t\t Pontuação\t\t Dono\t\t");
                            foreach (Game game in login.Cadastro.Games)
                            {
                                Console.WriteLine($"{game.Id}\t\t {game.Wave}\t\t {game.Name}\t\t {game.Score}\t\t {login.Cadastro.Name}");
                            }
                            break;

                        case 5:

                            List<(double score, string nome)> placar = new List<(double score, string nome)>();

                            foreach (Cadastro cad in cadastros)
                            {
                                cad.Score = cad.calculaScore(cad.Games);
                                placar.Add((cad.Score, cad.Name));
                            }

                            placar.Sort();
                            placar.Reverse();

                            int index = 1;

                            Console.WriteLine("Posição\t Pontuação\t Jogador");
                            foreach (var item in placar)
                            {
                                Console.WriteLine($"{index}\t {item.score}\t {item.nome}");
                                index += 1;
                            }

                            break;

                        case 6:
                            break;
                        default:
                            Console.WriteLine("O valor digitado não existe!");
                            break;
                    }
                    if (op == 6)
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Os dados digitados estão incorretos!");
                }
            }
            Console.WriteLine("Processo Encerrado!");
        }

    }
    class Cadastro
    {
        private int id;
        private string name;
        private string email;
        private string password;
        private static int cont = 0;
        private double score;
        public List<Game> Games { get; set; }
        public int Id { get { return id; } }
        public string Name { get { return name; } set { this.name = value; } }
        public string Email { get { return email; } set { this.email = value; } }
        public string Password { get { return password; } set { this.password = value; } }
        public double Score { get; set; }

        public Cadastro(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("There must be a name.");
            }
            this.name = name;
            this.email = email;
            this.password = password;
            Random rnd = new Random();
            id = 1000 + cont;
            cont += 1;
            Games = new List<Game>();
            score = 0;
        }

        public Cadastro()
        {

        }

        public double calculaScore(List<Game> games)
        {
            this.score = 0;
            foreach (Game game in games)
            {
                score += game.Score;
            }
            return score;
        }

    }

    class Login
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public Cadastro Cadastro { get; set; }
    }

    class Game
    {
        private int id;
        private string name;
        private double score;
        private static int cont;
        private int wave;
        public int Id { get { return this.id; } }
        public string Name { get { return this.name; } set { this.name = value; } }
        public double Score { get { return this.score; } set { this.score = value; } }
        public int Wave { get { return wave; } }

        public Game(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("There must be a name.");
            }
            id = 1000 + cont;
            cont += 1;
            this.name = name;
            score = 0;
            wave = 1;
        }

        public void Play()
        {
            Random rnd = new Random();

            int result;
            int answer;
            int tries;

            while (true)
            {
                Console.WriteLine("Nível: " + wave);
                tries = 1;
                result = (int)(rnd.Next() % Math.Round(Math.Pow(wave, 3) * 10));

                while (true)
                {
                    Console.WriteLine("Chute um número: ");
                    try
                    {
                        answer = int.Parse(Console.ReadLine());
                        if (answer < result)
                        {
                            Console.WriteLine("O número é maior!");
                            tries += 1;
                            continue;
                        }
                        if (answer > result)
                        {
                            Console.WriteLine("O número é menor!");
                            tries += 1;
                            continue;
                        }

                        Console.WriteLine("Número correto!");
                        wave += 1;
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }

                score += wave * 100 / tries;

                Console.WriteLine("Jogar novamente:\n\n 1.Sim\n 2.Não\n ");
                int op = int.Parse(Console.ReadLine());
                if (op == 1)
                {
                    continue;
                }
                return;
            }
        }
    }

}