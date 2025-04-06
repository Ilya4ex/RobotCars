#include "stm32f4xx_hal.h"

void setPWMRightD(uint16_t pwm_value);
void setPWMRightR(uint16_t pwm_value);
void setPWMLeftD(uint16_t pwm_value);
void setPWMLeftR(uint16_t pwm_value);
void changePwmRightD(uint16_t value);
void changePWMLeftD(uint16_t value);
void changePwmRightR(uint16_t value);
void changePWMLeftR(uint16_t value);
void startPwmRightD(void);
void startPWMLeftD(void);
void startPwmRightR(void);
void startPWMLeftR(void);
void stopPwmRightD(void);
void stopPWMLeftD(void);
void stopPwmRightR(void);
void stopPWMLeftR(void);
void StartDrive(uint16_t pwm_value);
void StartReverse(uint16_t pwm_value);
void StopDrive(void);
void StopReverse(void);
void Stop(void);
void test(uint8_t* strRxBuf);