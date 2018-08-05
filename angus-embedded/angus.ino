#include <avr/sleep.h>
#include <Wire.h>
#include <ATT_LoRaWAN.h>
#include <MicrochipLoRaModem.h>
#include <PayloadBuilder.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include "keys.h"

#define SERIAL_BAUD 57600

#define POWERSWITCH_PIN 22
#define BUTTON_PIN 20
#define LED 9

#define loraSerial  Serial1
#define gpsSerial   Serial

typedef struct {
  char lat[13];
  char NSInd[2];
  char lon[13];
  char EWInd[2];
} pos;

MicrochipLoRaModem modem(&loraSerial, NULL);
ATTDevice device(&modem, NULL, false, 7000);  // minimum time between 2 consecutive messages set to 7000 milliseconds
PayloadBuilder payload(device);

bool canSendData = false;
bool gpsReady = false;
unsigned char buf[64];
int count=0;
uint8_t *data;

pos gpsPosition;

Adafruit_BME280 bme;

volatile uint8_t watchdogPeriod = 0;
volatile bool sampleAndSend = false;

bool sleeping = false;

// ISRs
ISR(WDT_vect)
{
  if (++watchdogPeriod == 75) // 75 ticks = approx. 10 minutes delay, WDT period is 8 seconds
  {
    sampleAndSend = true;
    watchdogPeriod = 0;
  }
}

void setup() 
{
  pinMode(LED, OUTPUT);
  pinMode(POWERSWITCH_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT);

  loraSerial.begin(modem.getDefaultBaudRate());
  while((!loraSerial) && (millis()) < 10000); // wait until the serial bus is available
  while(!device.initABP(DEV_ADDR, APPSKEY, NWKSKEY));

  gpsSerial.begin(9600);
  while((!gpsSerial) && (millis()) < 15000); // wait until the serial bus is available

  setupTimer();
  
  bme.begin();

  // Sample at boot
  sampleAndSend = true;
}

void loop() 
{
  if (sampleAndSend == true)
  {
    sampleAndSend = false;
    sleeping = false;
    startGPS();
    sampleAndSendData();
  }
  else
  {
    sleepNow();
  }
}

void setupTimer()
{
  cli();
  // Watchdog Timer caused a reset?
  if (MCUSR & (1 << WDRF))
  {
    // Clear the WDT reset flag
    MCUSR &= ~(1 << WDRF);
    // Enable the WD Change Bit and WDI to have a soft reset
    WDTCSR |= ((1 << WDCE) | (1 << WDE));
    // Disable Watchdog timer
    WDTCSR = 0x00;
  }

  // Set up Watchdog Timer:
  // Authorize changes in WDTCSR
  WDTCSR |= ((1 << WDCE) | (1 << WDE));
  // Enable Watchdog with prescaler of 1024 (interrupt every 8s (approx))
  WDTCSR = (1 << WDIE) | (1 << WDP3) | (1 << WDP0);
  sei();
}

void sleepNow()
{
  if (!sleeping)
  {
    digitalWrite(POWERSWITCH_PIN, LOW);
    sleeping = true;
  }

  set_sleep_mode(SLEEP_MODE_PWR_SAVE);
  sleep_enable();
  sleep_mode(); // Put the device to sleep

  // Upon waking up, sketch continues from this point.
  sleep_disable();
}

/*
 * Enables the GPS
 */
void startGPS()
{
  digitalWrite(POWERSWITCH_PIN, HIGH);
  delay(30000);
}

void sendMockData()
{
  payload.reset();
  payload.addBoolean(0x32);
  payload.addBoolean(0x24);
  payload.addBoolean(0x0B);
  payload.addBoolean(0x1B);
  payload.addBoolean(0x4E);
  payload.addBoolean(0x03);
  payload.addBoolean(0x1E);
  payload.addBoolean(0x46);
  payload.addBoolean(0x38);
  payload.addBoolean(0x45);
  
  payload.addBoolean(0x25);
  payload.addBoolean(0x05);
  
  payload.addBoolean(0x00);
  
  payload.addBoolean(0x59);
  payload.addBoolean(0x5A);
  payload.addBoolean(0x5B);

  payload.addToQueue(false);
  process();
}

/*
 * Get GPS position, temperature, mock the rest of the data and prepare the paylod for LoRa message
 */
void sampleAndSendData()
{
  bool res = sampleGPSPosition();
  float temperature = bme.readTemperature();
  int8_t tempInteger = (int8_t) temperature;
  uint8_t tempDecimals = (int8_t) ((temperature - tempInteger) * 100);

  if (res)
  {
    char strByteHolder[3];
    uint8_t degree = 0;
    uint8_t minute = 0;
    uint8_t secondH = 0;
    uint8_t secondL = 0;
    
    sprintf(strByteHolder, "%c%c", gpsPosition.lat[3], gpsPosition.lat[4]);
    degree = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lat[5], gpsPosition.lat[6]);
    minute = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lat[8], gpsPosition.lat[9]);
    secondH = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lat[10], gpsPosition.lat[11]);
    secondL = atoi(strByteHolder);
   
    payload.reset();
    payload.addBoolean(degree);
    payload.addBoolean(minute);
    payload.addBoolean(secondH);
    payload.addBoolean(secondL);
    payload.addBoolean(0x4E); // // Always North! TODO: Change to handle at other locations
  
    sprintf(strByteHolder, "%c%c", gpsPosition.lon[3], gpsPosition.lon[4]);
    degree = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lon[5], gpsPosition.lon[6]);
    minute = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lon[8], gpsPosition.lon[9]);
    secondH = atoi(strByteHolder);
    sprintf(strByteHolder, "%c%c", gpsPosition.lon[10], gpsPosition.lon[11]);
    secondL = atoi(strByteHolder);
    
    payload.addBoolean(degree);
    payload.addBoolean(minute);
    payload.addBoolean(secondH);
    payload.addBoolean(secondL);
    payload.addBoolean(0x45); // Always East! TODO: Change to handle at other locations
    
    payload.addBoolean(tempInteger);
    payload.addBoolean(tempDecimals);

    // This is mock data, keep for future use and API compatibilty
    payload.addBoolean(0x00);
    
    payload.addBoolean(0x59);
    payload.addBoolean(0x5A);
    payload.addBoolean(0x5B);
  
    payload.addToQueue(false);
    process();
    blinkLedOk();
  }
}

/*
 * Sample GPS position from GPS serial NMEA messages (see chipset doc. for more information)
 */
bool sampleGPSPosition()
{
  char gpgga_array[9][13];
  String s = "";
  int strLen = 0;
  int attempts = 0;

  do 
  {
    s = gpsSerial.readStringUntil('\n');
    strLen = s.length();
  } while((!s.startsWith("$GPGGA") || strLen < 51 || strLen > 80) && attempts++ < 100);
  
  char gpgga[strLen + 1];

  for (int i = 0; i < strLen; i++)
  {
    gpgga[i] = s[i];
  }
  
  gpgga[strLen] = '\0';
  
  split_string(gpgga+7, strLen, gpgga_array, 10);

  if (strLen > 51 && strLen < 80)
  {
    gpsReady = true;  
    sprintf(gpsPosition.lat, "%12s", gpgga_array[1]);
    sprintf(gpsPosition.NSInd, "%1s", gpgga_array[2]);
    sprintf(gpsPosition.lon, "%12s", gpgga_array[3]);
    sprintf(gpsPosition.EWInd, "%1s", gpgga_array[4]);

    #ifdef debug
    loraSerial.print("lat:");loraSerial.println(gpsPosition.lat);
    loraSerial.print("NS:");loraSerial.println(gpsPosition.NSInd);
    loraSerial.print("lon:");loraSerial.println(gpsPosition.lon);
    loraSerial.print("EW:");loraSerial.println(gpsPosition.EWInd);
    #endif
    
    return true;
  }
  else
  {
    return false;
  }
}

/*
 * Used to split the GPS coordinates string into chunks for pos struct
 */
void split_string(const char *data, int data_len, char holder[][13], int max_len) {
  int holder_index = 0;
  int a = 0;
  
  for (int i = 0; i < data_len && holder_index < max_len; i++) {        
    if (data[i] == '\0') {
      return;
    }
    
    if (data[i] != ',') {
      holder[holder_index][a++] = data[i];
    }
    else {
      holder[holder_index++][a] = '\0';
      a = 0;
      continue;
    }
  }
}

/*
 * Used to send the LoRa message
 */
void process()
{
  while(device.processQueue() > 0)
  {
    device.queueCount();
  }
}

/*
 * Indicates OK status by flashing quickly
 */
void blinkLedOk()
{
  for (int i = 0; i < 10; i++)
  {
    digitalWrite(LED, LOW);
    delay(50);
    digitalWrite(LED, HIGH);
    delay(50);
  }

  digitalWrite(LED, LOW);
}

/*
 * Indicates OK status by flashing slowly
 */
void blinkLedFailure()
{
  for (int i = 0; i < 2; i++)
  {
    digitalWrite(LED, LOW);
    delay(500);
    digitalWrite(LED, HIGH);
    delay(500);
  }

  digitalWrite(LED, LOW);
}

