#include <EEPROM.h>
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include <SoftwareSerial.h>   // We need this even if we're not using a SoftwareSerial object
#include <SerialCommand.h>

#define highByte(x) ( (x) >> (8) ) // keep upper 8 bits
#define lowByte(x) ( (x) & (0xff) ) // keep lower 8 bits

SerialCommand SCmd;   // The SerialCommand object
Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

int _analogPin = 0;  //Initialice the analog Port
int _val=0;

int _wait;           //Wait Time between every Step
int _max;            //Var to count the next step
int _presure =0;     //presure for gripper

int _newPos[6];      //the next Position per Servo
int _actualPos[6];   //the actual Position per Servo
int _direction[6];   //the Direction for the next Move for every Servo
int _minValue[6];    //the nminimum Puls so that the Servo is in 0 Degree Position
int _maxValue[6];    //the maximum Puls so that the Servo is in 180 Degree Position
int _diff[6];        //Counter Var to calculate the next Spep per Servo

void setup()
{
  
  // Setup callbacks for SerialCommand commands
  SCmd.addCommand("?", Help);
  SCmd.addCommand("ADJ", Adjust);
  SCmd.addCommand("SMI", SetMinValue);
  SCmd.addCommand("SMA", SetMaxValue);
  SCmd.addCommand("MEP", MoveToEepromPos);
  SCmd.addCommand("HLD", Hold);
  SCmd.addCommand("INF", InfoServos);
  SCmd.addCommand("IMI", InfoMinValues);
  SCmd.addCommand("IMA", InfoMaxValues);
  SCmd.addCommand("IME", InfoMemoryValues);
  SCmd.addCommand("ISP", InfoStartValues);
  SCmd.addCommand("MAS", MoveAll);
  SCmd.addCommand("WEP", WritePosToEeprom);
  SCmd.addCommand("ONE", MoveOne);
  SCmd.addCommand("AAS", AdjustAll);
  SCmd.addCommand("PAS", PositionAll);
  SCmd.addCommand("SSP", SetStartPosition);
  SCmd.addCommand("MSP", MoveSpeed);
  SCmd.addCommand("WAI", Wait);
  SCmd.addCommand("GEP", GetPresure);  
  SCmd.addCommand("SEP", SetPresure);
  SCmd.addCommand("MEM", Mem);            
  SCmd.addCommand("CLR", Clr);          
  SCmd.addDefaultHandler(unrecognized);		

  //Get Wait Time
  _wait = EEPROM.read(1);

  pwm.begin();         //Start Servo Board
  pwm.setPWMFreq(60);  // Analog servos run at ~60 Hz updates
  
  //Get Settings from EEPROM
  for (int i=0;i<6;i++)
  {
    _minValue[i] = EEPROMReadInt((i+1)*2);
    _maxValue[i] = EEPROMReadInt((i+1)*2+12);
    _actualPos[i] = EEPROM.read((i+1)+25);
    _newPos[i] = EEPROM.read((i+1)+25);
    _diff[i] = 0;
  }

  //Set Start Position for al Servos
  for(int i=0; i<6; i++)
  {
      pwm.setPWM(i, 0, map(_actualPos[i], 0, 180, _minValue[i], _maxValue[i]));

//    Serial.print(F("Servo: "));
//    Serial.print(i+1);
//    Serial.print(F(" Min: "));
//    Serial.print(_minValue[i]);
//    Serial.print(F(" Max: "));
//    Serial.print(_maxValue[i]);
//    Serial.print(F(" Actual: "));
//    Serial.print(_actualPos[i]);
//    Serial.print(F(" New: "));
//    Serial.print(_newPos[i]);
//    Serial.print(F(" diff: "));
//    Serial.println(_diff[i]);
  }

  //Init serial communication
  Serial.begin(19200);
  while (!Serial)    // Only Leonardo need this Part of Code
  {
  };

  //Show initial Info
  Serial.println(F("Servo Controller V 1.1"));

}

void loop()
{
  SCmd.readSerial();     // We don't do much, just process serial commands
}

// Read Int Value from EEPTROM
int EEPROMReadInt(int address)
{
  return EEPROM.read(address) + EEPROM.read(address+1)*256;
}

//Write Int Value to EEPROM
void EEPROMWriteInt(int address, int value)
{
  EEPROM.write(address, lowByte(value));
  EEPROM.write(address + 1, highByte(value));
}

// Show Help Screen
void Help()
{
  Serial.println(F("Servo Controller V 1.0"));
  Serial.println(F("-------------------------------------------------------------------------------"));
  Serial.println(F("Commands:"));
  Serial.println(F("AAS n0 n1 n2 n3 n4 n5  : adjust all Servos to Position 'nx'"));
  Serial.println(F("ADJ s n                : set Position for Servo 's' to 'n' (0 - 180) no Move"));
  Serial.println(F("CLR n                  : clear EEPROM from 20 - n"));
  Serial.println(F("GEP                    : read analog Value from Port 0"));
  Serial.println(F("HLD                    : write Wait Time to EEPROM"));
  Serial.println(F("INF                    : shows the Servo Settings and Wait Time"));
  Serial.println(F("IMI                    : shows the Minimum Values per Servo"));
  Serial.println(F("IMA                    : shows the Maximum Values per Servo"));
  Serial.println(F("ISP                    : shows the Start Position per Servo"));
  Serial.println(F("IME n                  : shows Servo Positions at Memory Position 'n'")); 
  Serial.println(F("MAS                    : Move all Servos to new Positions."));
  Serial.println(F("                         if presure is != n5 will stop if presure is arrived"));  
  Serial.println(F("MEM n                  : Shwos the Values in EEPROM from 1 - n"));
  Serial.println(F("MEP n                  : move all Servos to Memory Position 'n' (0-20)"));
  Serial.println(F("                         0 = Start Position"));
  Serial.println(F("MSP s n                : move Servo 's' in Speed Mode to Position 'n' (0-180)"));
  Serial.println(F("WEP n                  : write Servo Settings to Memory Position 'n' (1-20)"));
  Serial.println(F("ONE s n                : move Servo 's' to Position 'n'"));
  Serial.println(F("                         if presure is != n5 will stop if presure is arrived"));  
  Serial.println(F("PAS n0 n1 n2 n3 n4 n5  : adjust all Servos to Position 'nx' and move"));
  Serial.println(F("                         if presure is != n5 will stop if presure is arrived"));
  Serial.println(F("SMI s n                : set min Value for Servo 's' to Position 'n'."));
  Serial.println(F("                         multiply by 4us. Default = 200 => 854us"));
  Serial.println(F("SMA s n                : set max Value for Servo 's' to Position 'n'"));
  Serial.println(F("                         multiple by 4us. Default 580 => 2360us"));
  Serial.println(F("SSP s n                : set Start Position for Servo 's' to 'n'"));
  Serial.println(F("WAI n                  : set Wait Time between each Step 'n' in ms. Def. 10ms"));
  Serial.println(F("?                      : show Help"));
  Serial.println(F(""));
}

void GetPresure()
{
  _val = analogRead(_analogPin);
  Serial.println(_val);
}

void SetPresure()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }
  _presure = n;
  writeAck();
}

void writeAck()
{
  Serial.println("ACK");
}

//Clears EEPROM Cells 
void Clr()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }
  
  if(n>512) n = 512;
  
  for (int i=32; i<=n;i++) // Cleras only User Settings for stored Positions
  {
    EEPROM.write(i, 255);
  }

  writeAck();
}


void Mem()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }

  for (int i=1; i<=n;i++)
  {
    Serial.print(F("Cell: "));
    Serial.print(i);
    Serial.print(F("   Value: "));
    Serial.println(EEPROM.read(i));
  }

}

void WritePosToEeprom()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }
  if (n>0 && n<=20) // 20 Positions are possible
  {
    for (int i = 0; i < 6; i++)
    {
      int cell = (26+i)+n*6;
      EEPROM.write(cell, _actualPos[i]);
    }
  }
  else
    Serial.println(F("NACK - Wrong Position"));
    
  writeAck();

}

void InfoMemoryValues()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }
  if (n>=0 && n<=20) // Moves to the stored Positions (0 = Start Position)
  {
    for (int i = 0; i < 6; i++)
    {
      int cell = (26+i)+n*6;
      Serial.print(EEPROM.read(cell));
      Serial.print(" ");
    }
    Serial.println("");
  }
  else
    Serial.println(F("NACK - Wrong Position"));  
  
}
void MoveToEepromPos()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }
  if (n>=0 && n<=20) // Moves to the stored Positions (0 = Start Position)
  {
    for (int i = 0; i < 6; i++)
    {
      int cell = (26+i)+n*6;
      SetNewPos(i, EEPROM.read(cell));
    }
  }
  else
    Serial.println(F("NACK - Wrong Position"));

  MoveAll();
}

void SetStartPosition()
{
  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  if (s>=0 && s<6)
  {
    EEPROM.write((s+1)+25,n); //Set the Start Position for Servo 's'
    writeAck();
  }
  else
    Serial.println(F("NACK - Wrong Servo Number"));
}

//void MoveAll()
//{
//  int counter[6];
//
//  for (int i = 0; i < 6; i++)
//  {
//    counter[i] = (_max / 2); //Initialice the Counter wit the Half of the biggest Difference from all Servos
//    //  Serial.println(counter[i]);
//  }
//
//  //For every needed Step we do the Sequence
//  for (int j = 1; j <= _max; j++) 
//  {
//    //    Serial.print(F("Step: "));
//    //    Serial.println(j);
//
//    //For every Servo
//    for (int i = 0; i < 6; i++)
//    {
//      //If we have a pos. Value substract the diff, otherwise add the Max Value - Diff
//      if (counter[i] >= 0)
//        counter[i] -= _diff[i];
//      else
//        counter[i] += _max - _diff[i];
//
//      //Correct the direction of the Move and write the next Position to the Servo
//      if(counter[i] < 0)
//        _actualPos[i] += 1 * _direction[i];
//
//      pwm.setPWM(i, 0, map(_actualPos[i], 0, 180, _minValue[i], _maxValue[i]));
//
//      //      Serial.print(F("Servo: "));
//      //      Serial.print(i+1);
//      //      Serial.print(F(" newPos: "));
//      //      Serial.print(_newPos[i]);
//      //      Serial.print(F(" Position: "));
//      //      Serial.println(_actualPos[i]);
//
//    }
//    delay(_wait);
//  }
//
//  //Clear Diff and Max because actualPos == newPos
//  for (int i = 0; i < 6; i++)
//  {
//    _diff[i] = 0;
//  }
//  _max = 0;
//
//  writeAck();
//
//}

void MoveAll()
{
  
  int counter[6];

  for (int i = 0; i < 6; i++)
  {
    counter[i] = (_max / 2); //Initialice the Counter wit the Half of the biggest Difference from all Servos
    //  Serial.println(counter[i]);
  }

  //For every needed Step we do the Sequence
  for (int j = 1; j <= _max; j++) 
  {
    //    Serial.print(F("Step: "));
    //    Serial.println(j);

    //For every Servo
    for (int i = 0; i < 6; i++)
    {
      //If we have a pos. Value substract the diff, otherwise add the Max Value - Diff
      if (counter[i] >= 0)
        counter[i] -= _diff[i];
      else
        counter[i] += _max - _diff[i];

      //Correct the direction of the Move and write the next Position to the Servo
      if(counter[i] < 0)
        _actualPos[i] += 1 * _direction[i];
      
      if (i == 5 && _direction[i] == 1)
      {
       
        _val = analogRead(_analogPin);

//        Serial.print("Servo 5 - ");
//        Serial.print("Soll: ");
//        Serial.println(presure);
//        Serial.print("Ist: ");
//        Serial.println(_val);
                
        if (_val < _presure)
        {
//          Serial.println(F("Servo 5 Move"));
          pwm.setPWM(i, 0, map(_actualPos[i], 0, 180, _minValue[i], _maxValue[i]));
        }
        else
        {
          _newPos[5] = _actualPos[5]; 
          _diff[5] = 0;
          
//          Serial.print(F("Servo: "));
//          Serial.print(i);
//          Serial.print(F(" newPos: "));
//          Serial.print(_newPos[i]);
//          Serial.print(F(" Position: "));
//          Serial.println(_actualPos[i]);
        }
      }
      else
      {
          pwm.setPWM(i, 0, map(_actualPos[i], 0, 180, _minValue[i], _maxValue[i]));
      }
      
//            Serial.print(F("Servo: "));
//            Serial.print(i);
//            Serial.print(F(" newPos: "));
//            Serial.print(_newPos[i]);
//            Serial.print(F(" Position: "));
//            Serial.println(_actualPos[i]);

    }
    delay(_wait);
  }

  //Clear Diff and Max because actualPos == newPos
  for (int i = 0; i < 6; i++)
  {
    _diff[i] = 0;
  }
  _max = 0;

  writeAck();

}

void InfoServos()
{
  for (int i = 0; i < 6; i++)
  {
    Serial.print(_actualPos[i]);
    Serial.print(F(" "));
  }
  Serial.println(_wait);
}

void InfoMinValues()
{
  for (int i = 0; i < 6; i++)
  {
    Serial.print(EEPROMReadInt((i+1)*2));
    Serial.print(F(" "));
  }
  Serial.println(F(""));
}

void InfoMaxValues()
{
  for (int i = 0; i < 6; i++)
  {
    Serial.print(EEPROMReadInt((i+1)*2+12));
    Serial.print(F(" "));
  }
  Serial.println(F(""));
}

void InfoStartValues()
{

  for (int i = 0; i < 6; i++)
  {
    Serial.print(EEPROM.read((i+1)+25));
    Serial.print(F(" "));
  }
  Serial.println(F(""));
}

void SetMinValue()
{
  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  if (s>=0 && s<6)
  {
    _minValue[s] = n;
    EEPROMWriteInt((s+1)*2,n);

//    Serial.print(F("Servo: "));
//    Serial.print(s);
//    Serial.print(F(" min: "));
//    Serial.print(EEPROMReadInt(s*2));
//    Serial.print(F(" Pin: "));
//    Serial.println(_pin[s-1]);

  writeAck();

  }
  else
    Serial.println(F("NACK - Wrong Servo Number"));

}

void SetMaxValue()
{
  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  if (s>=0 && s<6)
  {
    _maxValue[s] = n;
    EEPROMWriteInt((s+1)*2+12,n);
    writeAck();

//    Serial.print(F("Servo: "));
//    Serial.print(s);
//    Serial.print(F(" max: "));
//    Serial.print(EEPROMReadInt(s*2+12));
//    Serial.print(F(" Pin: "));
//    Serial.println(_pin[s-1]);
//    writeAck();

  }
  else
    Serial.println(F("NACK - Wrong Servo Number"));

}

//writes the Wait Time to the EEPROM
void Hold()
{
  EEPROM.write(1, _wait);
  writeAck();
}

//set the Wait Time (not permanentely)
void Wait()
{
  int n = 0;
  int test = CheckArgument(&n);
  if (test > 0)
  {
    return;
  }

  _wait = n;

  writeAck();
}

void MoveSpeed()
{
  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  SetNewPos(s, n);
  
  //only Values between 0 an 180 are alowed
  if (n<0){
    n=0;
  }
  if (n>180){
    n=180;
  }
  
  if(s>=0 && s<6)
  {
  //Set Endposition and MoveAll direct without delay
  _actualPos[s] = n;
  pwm.setPWM(s, 0, map(_actualPos[s], 0, 180, _minValue[s], _maxValue[s]));

  writeAck();
  }
  else
    Serial.println(F("NACK - Wrong Servo Number"));

}

void MoveOne()
{

  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  SetNewPos(s, n);
  
  // Move the Servo in normal speed to the Position
  MoveAll();
}

void Adjust()
{
  int s = 0;
  int n = 0;
  int test = CheckArguments(&s, &n);
  if (test > 0)
  {
    return;
  }

  SetNewPos(s, n);

  //  Serial.print(F("A "));
  //  Serial.print(s);
  //  Serial.print(F(" "));
  //  Serial.println(n);

  writeAck();
}

void PositionAll()
{
  int newPos[6];
  char *arg;

  for(int i=0; i<6; i++)
  {
    arg = SCmd.next();
    if (arg != NULL)
    {
      SetNewPos(i, atoi(arg));
    }
    else {
      Serial.println(F("NACK - Not enough arguments"));
      return;
    }
  }

  MoveAll();
}

void AdjustAll()
{
  int newPos[6];
  char *arg;

  for(int i=0; i<6; i++)
  {
    arg = SCmd.next();
    if (arg != NULL)
    {
//      SetNewPos(i+1, atoi(arg));
      newPos[i]=atoi(arg);
    }
    else {
      Serial.println(F("NACK - Not enough arguments"));
      return;
    }
  }

  writeAck();
}

void SetNewPos(int s, int n)
{
  if (n<0){
    n=0;
  }
  if (n>180){
    n=180;
  }

  if (s>=0 && s<6){
    _newPos[s] = n;

    if(_actualPos[s] > _newPos[s])
      _direction[s] = -1;
    else
      _direction[s] = 1;

    _diff[s] = (_actualPos[s] - _newPos[s]) * _direction[s] * -1;

  }
  else
    Serial.println(F("NACK - Wrong Servo Number"));

  //    Serial.print(F("Servo: "));
  //    Serial.print(s);
  //    Serial.print(F(" newPos: "));
  //    Serial.print(_newPos[s-1]);
  //    Serial.print(F(" actualPos: "));
  //    Serial.print(_actualPos[s-1]);
  //    Serial.print(F(" diff: "));
  //    Serial.print(_diff[s-1]);
  //    Serial.print(F(" Dir: "));
  //    Serial.println(_direction[s-1]);

  _max = 0;
  for(int i=0; i<6; i++)
  {
    if (_diff[i] > _max) _max = _diff[i];
  }

  //  Serial.print(F("Max: "));
  //  Serial.println(_max);
}

int CheckArgument(int *n)
{
  char *arg;
  arg = SCmd.next();
  if (arg != NULL)
  {
    *n=atoi(arg);    // Converts a char string to an integer
  }
  else {
    Serial.println(F("NACK - No arguments"));
    return 1;
  }
  return 0;
}

int CheckArguments(int *s, int *n)
{
  char *arg;
  arg = SCmd.next();
  if (arg != NULL)
  {
    *s=atoi(arg);    // Converts a char string to an integer
  }
  else {
    Serial.println(F("NACK - No arguments"));
    return 1;
  }

  arg = SCmd.next();
  if (arg != NULL)
  {
    *n=atol(arg);
  }
  else {
    Serial.println(F("NACK - No second argument"));
    return 2;
  }
  return 0;

}

// This gets set as the default handler, and gets called when no other command matches.
void unrecognized()
{
  Serial.println(F("NACK - Unknown Command!"));
}


