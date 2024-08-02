
using System;

namespace ExamSystem
{
    public abstract class Question : IComparable<Question>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Mark { get; set; }
        public Answer[] AnswerList { get; set; }
        public Answer RightAnswer { get; set; }

        protected Question(string header, string body, int mark, int answerCount)
        {
            Header = header;
            Body = body;
            Mark = mark;
            AnswerList = new Answer[answerCount];
        }

        public abstract void ShowDetails();

        public int CompareTo(Question other)
        {
            return string.Compare(Header, other.Header, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"{Header} - {Body}";
        }
    }

    public class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, int mark)
            : base(header, body, mark, 2) 
        {
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"True/False Question: {Header}\n{Body}");
            Console.WriteLine("1: True");
            Console.WriteLine("2: False");
        }
    }

    public class MCQQuestion : Question
    {
        public MCQQuestion(string header, string body, int mark, int answerCount)
            : base(header, body, mark, answerCount)
        {
        }

        public override void ShowDetails()
        {
            Console.WriteLine($"MCQ Question: {Header}\n{Body}");
            for (int i = 0; i < AnswerList.Length; i++)
            {
                if (AnswerList[i] != null)
                {
                    Console.WriteLine($"{i + 1}: {AnswerList[i].AnswerText}");
                }
            }
        }
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public Answer(int answerId, string answerText)
        {
            AnswerId = answerId;
            AnswerText = answerText;
        }
    }

    public abstract class Exam
    {
        public DateTime ExamTime { get; set; }
        public int NumberOfQuestions { get; set; }
        public Subject AssociatedSubject { get; set; }
        public Question[] Questions { get; set; }

        protected Exam(DateTime examTime, int numberOfQuestions, Subject subject)
        {
            ExamTime = examTime;
            NumberOfQuestions = numberOfQuestions;
            AssociatedSubject = subject;
            Questions = new Question[numberOfQuestions];
        }

        public abstract void ShowExam();
        public abstract void TakeExam();

        public void AddQuestion(Question question, int index)
        {
            if (index < NumberOfQuestions)
            {
                Questions[index] = question;
            }
            else
            {
                Console.WriteLine("Index out of range. Cannot add more questions.");
            }
        }
    }

    public class FinalExam : Exam
    {
        public FinalExam(DateTime examTime, int numberOfQuestions, Subject subject)
            : base(examTime, numberOfQuestions, subject)
        {
        }

        public override void ShowExam()
        {
            Console.WriteLine("Final Exam:");
            foreach (var question in Questions)
            {
                if (question != null)
                {
                    question.ShowDetails();
                    Console.WriteLine($"Right Answer: {question.RightAnswer.AnswerText}\n");
                }
            }
        }

        public override void TakeExam()
        {
            Console.WriteLine("Take the Final Exam:");
            int score = 0;

            foreach (var question in Questions)
            {
                if (question != null)
                {
                    question.ShowDetails();
                    Console.Write("Your answer: ");
                    int userAnswerIndex = int.Parse(Console.ReadLine()) - 1;

                    if (question.AnswerList[userAnswerIndex] == question.RightAnswer)
                    {
                        Console.WriteLine("Correct!\n");
                        score += question.Mark;
                    }
                    else
                    {
                        Console.WriteLine($"Wrong! The correct answer is: {question.RightAnswer.AnswerText}\n");
                    }
                }
            }

            Console.WriteLine($"Your total score: {score}");
        }
    }

    public class PracticalExam : Exam
    {
        public PracticalExam(DateTime examTime, int numberOfQuestions, Subject subject)
            : base(examTime, numberOfQuestions, subject)
        {
        }

        public override void ShowExam()
        {
            Console.WriteLine("Practical Exam:");
            foreach (var question in Questions)
            {
                if (question != null)
                {
                    question.ShowDetails();
                }
            }
        }

        public override void TakeExam()
        {
            Console.WriteLine("Take the Practical Exam:");
            int score = 0;

            foreach (var question in Questions)
            {
                if (question != null)
                {
                    question.ShowDetails();
                    Console.Write("Your answer: ");
                    int userAnswerIndex = int.Parse(Console.ReadLine()) - 1;

                    if (question.AnswerList[userAnswerIndex] == question.RightAnswer)
                    {
                        Console.WriteLine("Correct!\n");
                        score += question.Mark;
                    }
                    else
                    {
                        Console.WriteLine($"Wrong! The correct answer is: {question.RightAnswer.AnswerText}\n");
                    }
                }
            }

            Console.WriteLine($"Your total score: {score}");
        }
    }

    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Exam Exam { get; set; }

        public Subject(int subjectId, string subjectName)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
        }

        public void CreateExam(Exam exam)
        {
            Exam = exam;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
           
            Subject subject = InputSubjectDetails();

          
            Exam exam = ChooseExamType(subject);

            
            for (int i = 0; i < exam.NumberOfQuestions; i++)
            {
                exam.AddQuestion(InputQuestionDetails(), i);
            }

            subject.CreateExam(exam);

            
            exam.ShowExam();

           
            exam.TakeExam();
        }

        static Subject InputSubjectDetails()
        {
            Console.Write("Enter Subject ID: ");
            int subjectId = int.Parse(Console.ReadLine());
            Console.Write("Enter Subject Name: ");
            string subjectName = Console.ReadLine();
            return new Subject(subjectId, subjectName);
        }

        static Exam ChooseExamType(Subject subject)
        {
            Console.Write("Choose Exam Type (1 for Final Exam, 2 for Practical Exam): ");
            int examType = int.Parse(Console.ReadLine());
            Console.Write("Enter Exam Time (in minutes): ");
            int examTimeMinutes = int.Parse(Console.ReadLine());
            DateTime examTime = DateTime.Now.AddMinutes(examTimeMinutes);
            Console.Write("Enter Number of Questions for the Exam: ");
            int numberOfQuestions = int.Parse(Console.ReadLine());

            if (examType == 1)
            {
                return new FinalExam(examTime, numberOfQuestions, subject);
            }
            else if (examType == 2)
            {
                return new PracticalExam(examTime, numberOfQuestions, subject);
            }
            else
            {
                throw new Exception("Invalid exam type");
            }
        }

        static Question InputQuestionDetails()
        {
            Console.Write("Enter Question Type (1 for True/False, 2 for MCQ): ");
            int questionType = int.Parse(Console.ReadLine());
            Console.Write("Enter Question Header: ");
            string header = Console.ReadLine();
            Console.Write("Enter Question Body: ");
            string body = Console.ReadLine();
            Console.Write("Enter Question Mark: ");
            int mark = int.Parse(Console.ReadLine());

            Question question = null;

            if (questionType == 1)
            {
                question = new TrueFalseQuestion(header, body, mark);
                question.AnswerList[0] = new Answer(1, "True");
                question.AnswerList[1] = new Answer(2, "False");
                Console.Write("Enter the correct answer (1 for True, 2 for False): ");
                int correctAnswerIndex = int.Parse(Console.ReadLine()) - 1;
                question.RightAnswer = question.AnswerList[correctAnswerIndex];
            }
            else if (questionType == 2)
            {
                Console.Write("Enter number of possible answers: ");
                int answerCount = int.Parse(Console.ReadLine());
                question = new MCQQuestion(header, body, mark, answerCount);
                for (int i = 0; i < answerCount; i++)
                {
                    Console.Write($"Enter Answer {i + 1} Text: ");
                    string answerText = Console.ReadLine();
                    question.AnswerList[i] = new Answer(i + 1, answerText);
                }
                Console.Write("Enter the correct answer index: ");
                int correctAnswerIndex = int.Parse(Console.ReadLine()) - 1;
                question.RightAnswer = question.AnswerList[correctAnswerIndex];
            }

            return question;
        }
    }
}





