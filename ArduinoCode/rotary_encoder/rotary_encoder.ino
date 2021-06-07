#include <Arduino.h>
#include <SoftwareSerial.h>
const int rxPin = 9;
const int txPin = 8;

//instantiate serial object
SoftwareSerial BTSerial(rxPin, txPin);

//rotary encoder pins
const int pin_CLK = 4;
const int pin_DO = 5;
const int pin_CS = 6; 

//button input pin
const int pin_BTN = 7;

//encoder signal input values
int input_do;

int bitCounter = 9;
word current_Data = 0;
const int other_bits = 6;
int convertedVal;
int buttonVal;
String data_str = "";
String comma = ",";


void readBTN(){
  buttonVal = digitalRead(pin_BTN);
//  Serial.print("B: ");
 // Serial.println(buttonVal);
}

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
    //Serial.print(input_do);
    current_Data |= (input_do << bitCounter);    
    bitCounter--;
  }
 // Serial.print(" ");
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
  convertedVal = map(current_Data, 0, 1023, 0, 360);
  readBTN();
  data_str = buttonVal + comma + convertedVal;
  Serial.println(data_str);
  BTSerial.println(data_str);   //serial output will be button data,encoder data
}

void setup() {
  pinMode(pin_CLK, OUTPUT);
  pinMode(pin_DO, INPUT_PULLUP);
  pinMode(pin_CS, OUTPUT);
  pinMode(rxPin, INPUT);
  pinMode(rxPin, OUTPUT);
  pinMode(pin_BTN, INPUT);
  BTSerial.begin(9600);
  while(!BTSerial);  //wait for connection
  
  //set default pin states
  digitalWrite(pin_CLK, HIGH);
  digitalWrite(pin_CS, LOW);
  Serial.begin(9600);
}

void loop() {
  readData();
  delay(2); //measure angle data every 2 ms
}