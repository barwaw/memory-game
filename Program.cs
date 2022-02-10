// variables
Random random = new Random();
int wordsNumber;
int rowsNumber;
int columnsNumber;
int guessesNumber;
Tile[,] board;
string restart = "n";
string input = "";
int wordsFound = 0;
string difficulty = "";

// read words
string [] words = File.ReadAllLines("Words.txt");
List<string> chosenWords = new List<string>();

do
{
  // difficulty
  Console.Write("Choose difficulty(\"easy\" or \"hard\"): ");
  while (difficulty.ToLower() != "easy" && difficulty.ToLower() != "hard")
  {
    difficulty = Console.ReadLine();
  }

  if(difficulty == "easy")
  {
    wordsNumber = 4;
    rowsNumber = 2;
    columnsNumber = 4;
    guessesNumber = 10;
  }else
  {
    wordsNumber = 8;
    rowsNumber = 4;
    columnsNumber = 4;
    guessesNumber = 15;
  }

  // choose words
  for(int i = 0; i < wordsNumber; i++){

    int r = random.Next(0,words.Length-1);
    while (chosenWords.Contains(words[r]))
    {
      r = random.Next(0,words.Length-1);
    }
    chosenWords.Add(words[r]);
  }

  chosenWords.AddRange(chosenWords);

  // prepare word board
  board = new Tile[rowsNumber,columnsNumber];

  for (int i = 0; i < rowsNumber; i++)
  {
    for (int j = 0; j < columnsNumber; j++)
    {
      int r = random.Next(0,chosenWords.Count-1);
      board[i,j] = new Tile();
      board[i,j].word = chosenWords[r];
      chosenWords.RemoveAt(r);
      board[i,j].showing = false;
    }
  }

  // main game loop
  do
  {
    Draw();
    Input();
    int [] firstChoise = Choise(input);

    Draw();
    Input();
    int [] secondChoise = Choise(input);

    if(firstChoise == secondChoise) continue;

    Draw();

    if(!Evaluate(firstChoise, secondChoise))
    {
      // wrong choise
      Thread.Sleep(2000);
      guessesNumber--;
    }
    else 
    {
      //right choise
      wordsFound++;
    }
    
  } while (guessesNumber > 0 && wordsFound < wordsNumber);

  // game over
  Console.Clear();
  if(wordsFound == wordsNumber)
  {
    Console.WriteLine("You Win");
  }else
  {
    Console.WriteLine("You Lose");
  }
  Console.Write("Restart(y/n): ");
  restart = Console.ReadLine().ToLower();
} while (restart == "y");





// functions, classes
void Input()
{
  int[] coordinates;
  do
  {
    Console.Write("Choose word: ");
    input = Console.ReadLine().ToUpper();
    coordinates = ConvertCoordinates(input);
  } while (input.Length != 2 ||
  coordinates[0] >= rowsNumber ||
  coordinates[1] >= columnsNumber ||
  board[coordinates[0],coordinates[1]].showing == true);
}

int[] Choise(string input) 
{
  int [] coordinates = ConvertCoordinates(input);
  board[coordinates[0],coordinates[1]].showing = true;
  return coordinates;
}

bool Evaluate(int[] first, int[] second)
{
  if(board[first[0],first[1]].word == board[second[0],second[1]].word)
  {
    return true;
  }
  else
  {
    board[first[0],first[1]].showing = false;
    board[second[0],second[1]].showing = false;
    return false;
  }
}

int[] ConvertCoordinates(string c)
{
  int[] coordinates = {c[0]-65, c[1]-49};
  return coordinates;
}

void Draw()
{
  // header, number coordinates
  Console.Clear();
  Console.WriteLine($"level: {difficulty}");
  Console.WriteLine($"guesses: {guessesNumber}");
  Console.Write("  ");

  for (int i = 0; i < columnsNumber; i++)
  {
    if(board[0,i].showing)
    Console.Write((i + 1) + new string(' ', board[0,i].word.Length));
    else 
    Console.Write((i + 1) + new string(' ', 1));
  }

  Console.Write("\n");

  // rows of words
  for (int i = 0; i < rowsNumber; i++)
  {
    // letter coordinates
    Console.Write((char)(65 + i) + " ");

    for (int j = 0; j < columnsNumber; j++)
    {
      if(board[i,j].showing)
      Console.Write(board[i,j].word + " ");
      else
      Console.Write("X ");
    }
    Console.Write("\n");
  }
}

class Tile
{
  public string word = "";
  public bool showing;
}
