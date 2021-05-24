#include <SoftwareSerial.h>
const int rxPin = 9;
const int txPin = 8;
float potValue = 0;
int convertedVal;
//instantiate serial object
SoftwareSerial BTSerial(rxPin, txPin);


void setup() {
  pinMode(rxPin, INPUT);
  pinMode(rxPin, OUTPUT);
  BTSerial.begin(9600);
  Serial.begin(9600)l //for debugging
}

void loop() {
  potValue = analogRead(A0);
  convertedVal = map(potValue, 0, 1023, 0, 255);  //convert to 1 byte to be transmitted
  BTSerial.write(angle);
  Serial.println(angle);
}
