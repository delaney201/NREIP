#include <Arduino.h>
#include <SoftwareSerial.h>
const int rxPin = 9;
const int txPin = 8;

//instantiate serial object
SoftwareSerial BTSerial(rxPin, txPin);

//rotary encoder pins
const int pin_CLK = 6;
const int pin_DO = 5;
const int pin_CS = 4; 

//encoder signal input values
int input_do;

int bitCounter = 9;
word current_Data = 0;
const int other_bits = 6;


void readData(){
  digitalWrite(pin_CS, HIGH);
  digitalWrite(pin_CS, LOW);
  delay(0.0005); //tCLKFE (500 ns)
  current_Data = 0;
  while(bitCounter > -1){
    //data available 375ns after rising edge
    digitalWrite(pin_CLK, LOW);
    digitalWrite(pin_CLK, HIGH);
    delay(0.000375);
    input_do = digitalRead(pin_DO);
    Serial.print(input_do);
    current_Data |= (input_do << bitCounter);    
    bitCounter--;
  }
  Serial.print(" ");
  //extra clk ticks for status and parity bits
  for(int i = 0; i < other_bits; i++){
    digitalWrite(pin_CLK, LOW);
    digitalWrite(pin_CLK, HIGH);
    delay(0.000375);
  }
  bitCounter = 9;
  digitalWrite(pin_CLK, LOW);
  digitalWrite(pin_CLK, HIGH);
  delay(0.0001); //100 ns
  digitalWrite(pin_CS, HIGH);  //back to default
  Serial.println(current_Data);
  BTSerial.print(current_Data);
  BTSerial.print("\n");
}

void setup() {
  pinMode(pin_CLK, OUTPUT);
  pinMode(pin_DO, INPUT_PULLUP);
  pinMode(pin_CS, OUTPUT);
  pinMode(rxPin, INPUT);
  pinMode(rxPin, OUTPUT);
  BTSerial.begin(9600);
  while(!BTSerial);  //wait for connection
  
  //set default pin states
  digitalWrite(pin_CLK, HIGH);
  digitalWrite(pin_CS, LOW);
  Serial.begin(9600);
}

void loop() {
  readData();
  delay(500); //measure angle data every 500 ms
}
