using System;

namespace project_1_v4
{
    class Answer
    {
        public int AnswerId;
        public string AnswerText;

        public Answer(int id, string text)
        {
            AnswerId = id;
            AnswerText = text;
        }
    }

    class Student
    {
        public string Id;
        public string Name;

        public Student(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void ShowInfo()
        {
            Console.WriteLine("Student ID: " + Id);
            Console.WriteLine("Student Name: " + Name);
            Console.WriteLine(" ");
        }
    }

    abstract class Question
    {
        public string Header;
        public string Body;
        public int Mark;
        public Answer[] Answers;
        public int RightAnswerId;

        public Question(string header, string body, int mark)
        {
            Header = header;
            Body = body;
            Mark = mark;
        }

        public void PrintQuestion()
        {
            Console.WriteLine();
            Console.WriteLine(Header + " (Mark: " + Mark + ")");
            Console.WriteLine(Body);

            for (int i = 0; i < Answers.Length; i++)
                Console.WriteLine(Answers[i].AnswerId + ". " + Answers[i].AnswerText);

            Console.Write("Your Answer: ");
        }

        public string GetRightAnswerText()
        {
            for (int i = 0; i < Answers.Length; i++)
            {
                if (Answers[i].AnswerId == RightAnswerId)
                    return Answers[i].AnswerText;
            }
            return "";
        }
    }

    class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string body, int mark)
            : base("True/False Question", body, mark)
        {
            Answers = new Answer[]
            {
                new Answer(1,"True"),
                new Answer(2,"False")
            };
        }
    }

    class MCQQuestion : Question
    {
        public MCQQuestion(string body, int mark, Answer[] answers)
            : base("MCQ Question", body, mark)
        {
            Answers = answers;
        }
    }

    abstract class Exam
    {
        public Question[] Questions;
        public int CorrectAnswers;

        public Exam(int numberOfQuestions)
        {
            Questions = new Question[numberOfQuestions];
            CorrectAnswers = 0;
        }

        protected int ReadAnswer(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();
                int value;

                if (!int.TryParse(input, out value))
                {
                    Console.Write("Invalid input, enter a number: ");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.Write("Invalid choice, enter between " + min + " and " + max + ": ");
                    continue;
                }

                return value;
            }
        }

        public abstract void Start(Student student);
    }

    class FinalExam : Exam
    {
        public FinalExam() : base(8) { }

        public override void Start(Student student)
        {
            Console.Clear();
            Console.WriteLine("<<< Final Exam >>>");
            student.ShowInfo();

            int grade = 0;
            int total = 0;
            CorrectAnswers = 0;

            int[] studentAnswers = new int[Questions.Length];

            for (int i = 0; i < Questions.Length; i++)
            {
                Question q = Questions[i];
                total += q.Mark;

                q.PrintQuestion();
                int ans = ReadAnswer(1, q.Answers.Length);
                studentAnswers[i] = ans;

                if (ans == q.RightAnswerId)
                {
                    grade += q.Mark;
                    CorrectAnswers++;
                }
            }

            Console.Clear();
            Console.WriteLine("<<< Final Exam Results >>>");
            Console.WriteLine("Grade: " + grade + " / " + total);
            Console.WriteLine("Correct Answers: " + CorrectAnswers + " / " + Questions.Length);

            Console.WriteLine();
            Console.WriteLine("<<< Answers Review >>>");

            for (int i = 0; i < Questions.Length; i++)
            {
                Question q = Questions[i];
                string yourAnswerText = "";
                for (int j = 0; j < q.Answers.Length; j++)
                {
                    if (q.Answers[j].AnswerId == studentAnswers[i])
                        yourAnswerText = q.Answers[j].AnswerText;
                }

                Console.WriteLine();
                Console.WriteLine((i + 1) + ") " + q.Body);
                Console.WriteLine("Your Answer: " + yourAnswerText);
                Console.WriteLine("Correct Answer: " + q.GetRightAnswerText());

                if (studentAnswers[i] == q.RightAnswerId)
                    Console.WriteLine("Result: Correct");
                else
                    Console.WriteLine("Result: Wrong");
            }
        }
    }

    class PracticalExam : Exam
    {
        public PracticalExam() : base(8) { }

        public override void Start(Student student)
        {
            Console.Clear();
            Console.WriteLine("<<< Practical Exam >>>");
            student.ShowInfo();

            CorrectAnswers = 0;
            int[] studentAnswers = new int[Questions.Length];

            for (int i = 0; i < Questions.Length; i++)
            {
                Question q = Questions[i];

                q.PrintQuestion();
                int ans = ReadAnswer(1, q.Answers.Length);
                studentAnswers[i] = ans;

                if (ans == q.RightAnswerId)
                    CorrectAnswers++;
            }

            Console.Clear();
            Console.WriteLine("<<< Practical Exam Results >>>");
            Console.WriteLine("Correct Answers: " + CorrectAnswers + " / " + Questions.Length);
            Console.WriteLine();
            Console.WriteLine("<<< Answers Review >>>");

            for (int i = 0; i < Questions.Length; i++)
            {
                Question q = Questions[i];

                string yourAnswerText = "";
                for (int j = 0; j < q.Answers.Length; j++)
                {
                    if (q.Answers[j].AnswerId == studentAnswers[i])
                        yourAnswerText = q.Answers[j].AnswerText;
                }

                Console.WriteLine();
                Console.WriteLine((i + 1) + ") " + q.Body);
                Console.WriteLine("Your Answer: " + yourAnswerText);
                Console.WriteLine("Correct Answer: " + q.GetRightAnswerText());

                if (studentAnswers[i] == q.RightAnswerId)
                    Console.WriteLine("Result: Correct");
                else
                    Console.WriteLine("Result: Wrong");
            }
        }
    }

    class Subject
    {
        public int Id;
        public string Name;
        public Exam Exam;

        public Subject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void CreateExam(Exam exam)
        {
            Exam = exam;

            bool practical = (exam is PracticalExam);

            if (Name == "OOP") FillOOP(practical);
            else if (Name == "English") FillEnglish(practical);
            else if (Name == "Intro to CS") FillICS(practical);
            else if (Name == "Calculus") FillCalculus(practical);
        }

        private void FillOOP(bool practical)
        {
            if (practical)
            {
                Exam.Questions[0] = new MCQQuestion("Which keyword creates an object in C#?", 5,
                    new Answer[] { new Answer(1, "new"), new Answer(2, "class"), new Answer(3, "void"), new Answer(4, "using") })
                { RightAnswerId = 1 };

                Exam.Questions[1] = new MCQQuestion("Most restrictive access modifier?", 5,
                    new Answer[] { new Answer(1, "public"), new Answer(2, "private"), new Answer(3, "protected"), new Answer(4, "internal") })
                { RightAnswerId = 2 };

                Exam.Questions[2] = new MCQQuestion("Encapsulation means:", 5,
                    new Answer[] { new Answer(1, "Hiding details"), new Answer(2, "Code reuse"), new Answer(3, "Multiple forms"), new Answer(4, "Compilation") })
                { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("Inheritance symbol in C# is:", 5,
                    new Answer[] { new Answer(1, ":"), new Answer(2, "extends"), new Answer(3, "base"), new Answer(4, "this") })
                { RightAnswerId = 1 };

                Exam.Questions[4] = new MCQQuestion("Which cannot be instantiated?", 5,
                    new Answer[] { new Answer(1, "abstract class"), new Answer(2, "class"), new Answer(3, "struct"), new Answer(4, "enum") })
                { RightAnswerId = 1 };

                Exam.Questions[5] = new MCQQuestion("Method overloading is:", 5,
                    new Answer[] { new Answer(1, "Polymorphism"), new Answer(2, "Encapsulation"), new Answer(3, "Aggregation"), new Answer(4, "Serialization") })
                { RightAnswerId = 1 };

                Exam.Questions[6] = new MCQQuestion("Keyword for current object:", 5,
                    new Answer[] { new Answer(1, "this"), new Answer(2, "base"), new Answer(3, "static"), new Answer(4, "sealed") })
                { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("Keyword that prevents inheritance:", 5,
                    new Answer[] { new Answer(1, "sealed"), new Answer(2, "virtual"), new Answer(3, "override"), new Answer(4, "partial") })
                { RightAnswerId = 1 };
            }
            else
            {
                Exam.Questions[0] = new TrueFalseQuestion("Encapsulation is an OOP concept.", 5) { RightAnswerId = 1 };
                Exam.Questions[1] = new TrueFalseQuestion("Inheritance reduces code reuse.", 5) { RightAnswerId = 2 };

                Exam.Questions[2] = new MCQQuestion("Which keyword creates object in C#?", 5,
                    new Answer[] { new Answer(1, "new"), new Answer(2, "class"), new Answer(3, "void"), new Answer(4, "this") })
                { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("Which supports polymorphism?", 5,
                    new Answer[] { new Answer(1, "Method Overloading"), new Answer(2, "Arrays"), new Answer(3, "Loops"), new Answer(4, "Variables") })
                { RightAnswerId = 1 };

                Exam.Questions[4] = new TrueFalseQuestion("C# supports multiple inheritance.", 5) { RightAnswerId = 2 };

                Exam.Questions[5] = new MCQQuestion("Most restrictive access modifier?", 5,
                    new Answer[] { new Answer(1, "public"), new Answer(2, "private"), new Answer(3, "protected"), new Answer(4, "internal") })
                { RightAnswerId = 2 };

                Exam.Questions[6] = new TrueFalseQuestion("Abstract classes can have constructors.", 5) { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("Inheritance symbol in C# is:", 5,
                    new Answer[] { new Answer(1, "extends"), new Answer(2, ":"), new Answer(3, "base"), new Answer(4, "this") })
                { RightAnswerId = 2 };
            }
        }

        private void FillEnglish(bool practical)
        {
            if (practical)
            {
                Exam.Questions[0] = new MCQQuestion("She ___ to school every day.", 5,
                    new Answer[] { new Answer(1, "go"), new Answer(2, "goes"), new Answer(3, "going"), new Answer(4, "gone") })
                { RightAnswerId = 2 };

                Exam.Questions[1] = new MCQQuestion("Plural of 'child' is:", 5,
                    new Answer[] { new Answer(1, "childs"), new Answer(2, "children"), new Answer(3, "childes"), new Answer(4, "childrens") })
                { RightAnswerId = 2 };

                Exam.Questions[2] = new MCQQuestion("Correct article: ___ apple", 5,
                    new Answer[] { new Answer(1, "a"), new Answer(2, "an"), new Answer(3, "the"), new Answer(4, "none") })
                { RightAnswerId = 2 };

                Exam.Questions[3] = new MCQQuestion("Synonym of happy:", 5,
                    new Answer[] { new Answer(1, "sad"), new Answer(2, "angry"), new Answer(3, "joyful"), new Answer(4, "tired") })
                { RightAnswerId = 3 };

                Exam.Questions[4] = new MCQQuestion("Past tense of 'go' is:", 5,
                    new Answer[] { new Answer(1, "goed"), new Answer(2, "went"), new Answer(3, "goes"), new Answer(4, "going") })
                { RightAnswerId = 2 };

                Exam.Questions[5] = new MCQQuestion("Correct preposition: I am good ___ math.", 5,
                    new Answer[] { new Answer(1, "in"), new Answer(2, "at"), new Answer(3, "on"), new Answer(4, "to") })
                { RightAnswerId = 2 };

                Exam.Questions[6] = new MCQQuestion("Which is an adjective?", 5,
                    new Answer[] { new Answer(1, "quickly"), new Answer(2, "beautiful"), new Answer(3, "happiness"), new Answer(4, "run") })
                { RightAnswerId = 2 };

                Exam.Questions[7] = new MCQQuestion("Correct sentence:", 5,
                    new Answer[] { new Answer(1, "He don't like tea."), new Answer(2, "He doesn't likes tea."), new Answer(3, "He doesn't like tea."), new Answer(4, "He not like tea.") })
                { RightAnswerId = 3 };
            }
            else
            {
                Exam.Questions[0] = new TrueFalseQuestion("An adjective describes a noun.", 5) { RightAnswerId = 1 };

                Exam.Questions[1] = new MCQQuestion("She ___ to school.", 5,
                    new Answer[] { new Answer(1, "go"), new Answer(2, "goes"), new Answer(3, "going"), new Answer(4, "gone") })
                { RightAnswerId = 2 };

                Exam.Questions[2] = new TrueFalseQuestion("A sentence must have a verb.", 5) { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("Plural of child:", 5,
                    new Answer[] { new Answer(1, "childs"), new Answer(2, "children"), new Answer(3, "childes"), new Answer(4, "childrens") })
                { RightAnswerId = 2 };

                Exam.Questions[4] = new TrueFalseQuestion("Adverbs describe verbs.", 5) { RightAnswerId = 1 };

                Exam.Questions[5] = new MCQQuestion("Correct article: ___ apple", 5,
                    new Answer[] { new Answer(1, "a"), new Answer(2, "an"), new Answer(3, "the"), new Answer(4, "none") })
                { RightAnswerId = 2 };

                Exam.Questions[6] = new TrueFalseQuestion("Went is past tense.", 5) { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("Synonym of happy:", 5,
                    new Answer[] { new Answer(1, "sad"), new Answer(2, "angry"), new Answer(3, "joyful"), new Answer(4, "tired") })
                { RightAnswerId = 3 };
            }
        }

        private void FillICS(bool practical)
        {
            if (practical)
            {
                Exam.Questions[0] = new MCQQuestion("CPU stands for:", 5,
                    new Answer[] { new Answer(1, "Central Processing Unit"), new Answer(2, "Computer Personal Unit"), new Answer(3, "Central Program Unit"), new Answer(4, "Control Processing Unit") })
                { RightAnswerId = 1 };

                Exam.Questions[1] = new MCQQuestion("Which is input device?", 5,
                    new Answer[] { new Answer(1, "Monitor"), new Answer(2, "Printer"), new Answer(3, "Keyboard"), new Answer(4, "Speaker") })
                { RightAnswerId = 3 };

                Exam.Questions[2] = new MCQQuestion("RAM is:", 5,
                    new Answer[] { new Answer(1, "Volatile"), new Answer(2, "Non-volatile"), new Answer(3, "Secondary"), new Answer(4, "Permanent") })
                { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("Binary uses:", 5,
                    new Answer[] { new Answer(1, "0 and 1"), new Answer(2, "1 and 2"), new Answer(3, "0 and 2"), new Answer(4, "2 and 3") })
                { RightAnswerId = 1 };

                Exam.Questions[4] = new MCQQuestion("System software:", 5,
                    new Answer[] { new Answer(1, "Word"), new Answer(2, "Excel"), new Answer(3, "Windows"), new Answer(4, "Chrome") })
                { RightAnswerId = 3 };

                Exam.Questions[5] = new MCQQuestion("Temporary memory:", 5,
                    new Answer[] { new Answer(1, "ROM"), new Answer(2, "RAM"), new Answer(3, "HDD"), new Answer(4, "SSD") })
                { RightAnswerId = 2 };

                Exam.Questions[6] = new MCQQuestion("Secondary storage:", 5,
                    new Answer[] { new Answer(1, "RAM"), new Answer(2, "Cache"), new Answer(3, "Hard Disk"), new Answer(4, "Register") })
                { RightAnswerId = 3 };

                Exam.Questions[7] = new MCQQuestion("Binary number 10 equals:", 5,
                    new Answer[] { new Answer(1, "1"), new Answer(2, "2"), new Answer(3, "3"), new Answer(4, "4") })
                { RightAnswerId = 2 };
            }
            else
            {
                Exam.Questions[0] = new TrueFalseQuestion("CPU is the brain of the computer.", 5) { RightAnswerId = 1 };

                Exam.Questions[1] = new MCQQuestion("Which is input device?", 5,
                    new Answer[] { new Answer(1, "Monitor"), new Answer(2, "Printer"), new Answer(3, "Keyboard"), new Answer(4, "Speaker") })
                { RightAnswerId = 3 };

                Exam.Questions[2] = new TrueFalseQuestion("RAM is non-volatile.", 5) { RightAnswerId = 2 };

                Exam.Questions[3] = new MCQQuestion("Binary uses:", 5,
                    new Answer[] { new Answer(1, "0 and 1"), new Answer(2, "1 and 2"), new Answer(3, "0 and 2"), new Answer(4, "2 and 3") })
                { RightAnswerId = 1 };

                Exam.Questions[4] = new TrueFalseQuestion("Software is tangible.", 5) { RightAnswerId = 2 };

                Exam.Questions[5] = new MCQQuestion("System software:", 5,
                    new Answer[] { new Answer(1, "Word"), new Answer(2, "Excel"), new Answer(3, "Windows"), new Answer(4, "Chrome") })
                { RightAnswerId = 3 };

                Exam.Questions[6] = new TrueFalseQuestion("Hard disk is secondary storage.", 5) { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("Temporary memory:", 5,
                    new Answer[] { new Answer(1, "ROM"), new Answer(2, "RAM"), new Answer(3, "HDD"), new Answer(4, "SSD") })
                { RightAnswerId = 2 };
            }
        }

        private void FillCalculus(bool practical)
        {
            if (practical)
            {
                Exam.Questions[0] = new MCQQuestion("Derivative of x^2 is:", 5,
                    new Answer[] { new Answer(1, "x"), new Answer(2, "2x"), new Answer(3, "x^2"), new Answer(4, "2") })
                { RightAnswerId = 2 };

                Exam.Questions[1] = new MCQQuestion("Integral of 1 dx is:", 5,
                    new Answer[] { new Answer(1, "x + C"), new Answer(2, "1"), new Answer(3, "0"), new Answer(4, "x^2") })
                { RightAnswerId = 1 };

                Exam.Questions[2] = new MCQQuestion("d/dx(5x) is:", 5,
                    new Answer[] { new Answer(1, "5"), new Answer(2, "x"), new Answer(3, "5x"), new Answer(4, "0") })
                { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("Integral of x dx is:", 5,
                    new Answer[] { new Answer(1, "x^2"), new Answer(2, "x^2/2"), new Answer(3, "2x"), new Answer(4, "1") })
                { RightAnswerId = 2 };

                Exam.Questions[4] = new MCQQuestion("Limit of 1/x as x→∞ is:", 5,
                    new Answer[] { new Answer(1, "0"), new Answer(2, "∞"), new Answer(3, "1"), new Answer(4, "-∞") })
                { RightAnswerId = 1 };

                Exam.Questions[5] = new MCQQuestion("Derivative of a constant is:", 5,
                    new Answer[] { new Answer(1, "0"), new Answer(2, "1"), new Answer(3, "constant"), new Answer(4, "x") })
                { RightAnswerId = 1 };

                Exam.Questions[6] = new MCQQuestion("Integral is inverse of:", 5,
                    new Answer[] { new Answer(1, "Derivative"), new Answer(2, "Limit"), new Answer(3, "Function"), new Answer(4, "Series") })
                { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("Continuous function needs:", 5,
                    new Answer[] { new Answer(1, "Limit exists"), new Answer(2, "Infinity"), new Answer(3, "Zero"), new Answer(4, "No graph") })
                { RightAnswerId = 1 };
            }
            else
            {
                Exam.Questions[0] = new TrueFalseQuestion("Derivative of constant is zero.", 5) { RightAnswerId = 1 };

                Exam.Questions[1] = new MCQQuestion("Derivative of x^2 is:", 5,
                    new Answer[] { new Answer(1, "x"), new Answer(2, "2x"), new Answer(3, "x^2"), new Answer(4, "2") })
                { RightAnswerId = 2 };

                Exam.Questions[2] = new TrueFalseQuestion("Integral is inverse of derivative.", 5) { RightAnswerId = 1 };

                Exam.Questions[3] = new MCQQuestion("∫1 dx =", 5,
                    new Answer[] { new Answer(1, "x + C"), new Answer(2, "1"), new Answer(3, "0"), new Answer(4, "x^2") })
                { RightAnswerId = 1 };

                Exam.Questions[4] = new TrueFalseQuestion("Limit can be infinity.", 5) { RightAnswerId = 1 };

                Exam.Questions[5] = new MCQQuestion("d/dx(5x) =", 5,
                    new Answer[] { new Answer(1, "5"), new Answer(2, "x"), new Answer(3, "5x"), new Answer(4, "0") })
                { RightAnswerId = 1 };

                Exam.Questions[6] = new TrueFalseQuestion("Continuity needs limit.", 5) { RightAnswerId = 1 };

                Exam.Questions[7] = new MCQQuestion("∫x dx =", 5,
                    new Answer[] { new Answer(1, "x^2"), new Answer(2, "x^2/2"), new Answer(3, "2x"), new Answer(4, "1") })
                { RightAnswerId = 2 };
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Enter Your ID: ");
            string id = Console.ReadLine();

            Console.Write("Enter Your Name: ");
            string name = Console.ReadLine();

            Student student = new Student(id, name);

            Subject? subject = null;
            while (subject == null)
            {
                Console.WriteLine("\nChoose Subject:");
                Console.WriteLine("1. OOP");
                Console.WriteLine("2. English");
                Console.WriteLine("3. Intro to CS");
                Console.WriteLine("4. Calculus");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (choice == 1) subject = new Subject(1, "OOP");
                else if (choice == 2) subject = new Subject(2, "English");
                else if (choice == 3) subject = new Subject(3, "Intro to CS");
                else if (choice == 4) subject = new Subject(4, "Calculus");
                else Console.WriteLine("Invalid choice. Try again.");
            }

            Exam? exam = null;
            while (exam == null)
            {
                Console.WriteLine("\nChoose Exam Type.:");
                Console.WriteLine("1. Final.");
                Console.WriteLine("2. Practical.");

                int examChoice;
                if (!int.TryParse(Console.ReadLine(), out examChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (examChoice == 1) exam = new FinalExam();
                else if (examChoice == 2) exam = new PracticalExam();
                else Console.WriteLine("Invalid choice. Try again.");
            }

            subject.CreateExam(exam);

            exam.Start(student);

            Console.WriteLine("\nExam Finished.");
            Console.ReadKey();
        }
    }
}
