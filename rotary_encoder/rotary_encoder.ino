//rotary encoder pins
const int pin_CLK = 4;
const int pin_DO = 5;
const int pin_CS = 6; 

//encoder signal input values
int input_clk;
int input_do;
int input_cs;

int bitCounter = 9;
word current_Data = 0;
volatile int first_clk = 0;
volatile int collect_data = 0;

void clk_ISR(){
  if(first_clk){
    if(bitCounter > -1){
      input_do = digitalRead(pin_DO);
      currentData |= (input_do << bitCounter);    
      bitCounter--;
    }
  }
  else{
    first_clk = 1;
  }
}

void setup() {
  pinMode(pin_CLK, INPUT);
  pinMode(pin_DO, INPUT);
  pinMode(pin_CS, INPUT);
  Serial.begin(9600);
  //setup interrupt for falling clk signal (1st is ignored)
  attachInterrupt(digitalPinToInterrupt(pin_CLK), clk_ISR, FALLING);
}

void loop() {
  input_cs = digitalRead(pin_CS);
  if(input_cs == HIGH){  //encoder inactive -> reset globals
    first_clk = 0;
    bitCounter = 9;
    currentData = 0;
  }
 
  /*if(input_cs == LOW){
    //new angle to be read
    while(bitCounter < 10){
      //input_do = digitalRead(pin_DO);
      input_clk = digitalRead(pin_CLK);
      if(input_clk == LOW){
        
      }
      bitCounter++;
    }
    bitCounter = 0;
  }*/
}
