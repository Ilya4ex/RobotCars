#include "PWMEngineControl.h"

extern TIM_HandleTypeDef htim1;
extern TIM_HandleTypeDef htim2;
extern TIM_HandleTypeDef htim3;
extern TIM_HandleTypeDef htim4;
extern TIM_HandleTypeDef htim5;

void setPWMLeftD(uint16_t value)
{
	TIM_OC_InitTypeDef sConfigOC;
	
	sConfigOC.OCMode = TIM_OCMODE_PWM1;
	sConfigOC.Pulse = value;
	sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
	sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
	HAL_TIM_PWM_ConfigChannel(&htim2,&sConfigOC, TIM_CHANNEL_1);
}

void setPWMLeftR(uint16_t value)
{
	TIM_OC_InitTypeDef sConfigOC;
	
	sConfigOC.OCMode = TIM_OCMODE_PWM1;
	sConfigOC.Pulse = value;
	sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
	sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
	HAL_TIM_PWM_ConfigChannel(&htim3,&sConfigOC, TIM_CHANNEL_3);
}
void setPWMRightD(uint16_t value)
{
	TIM_OC_InitTypeDef sConfigOC;
	
	sConfigOC.OCMode = TIM_OCMODE_PWM1;
	sConfigOC.Pulse = value;
	sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
	sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
	HAL_TIM_PWM_ConfigChannel(&htim4,&sConfigOC, TIM_CHANNEL_1);
}

void setPWMRightR(uint16_t value)
{
	TIM_OC_InitTypeDef sConfigOC;
	
	sConfigOC.OCMode = TIM_OCMODE_PWM1;
	sConfigOC.Pulse = value;
	sConfigOC.OCPolarity = TIM_OCPOLARITY_HIGH;
	sConfigOC.OCFastMode = TIM_OCFAST_DISABLE;
	HAL_TIM_PWM_ConfigChannel(&htim5,&sConfigOC, TIM_CHANNEL_1);
}

void startPwmRightD()
{
	HAL_TIM_PWM_Start(&htim4,TIM_CHANNEL_1);	
}
void startPWMLeftD()
{
  HAL_TIM_PWM_Start(&htim2,TIM_CHANNEL_1);
}
void startPwmRightR()
{
	HAL_TIM_PWM_Start(&htim5,TIM_CHANNEL_1);
}
void startPWMLeftR()
{
  HAL_TIM_PWM_Start(&htim3,TIM_CHANNEL_3);
}
void stopPwmRightD()
{	
	HAL_TIM_PWM_Stop(&htim4,TIM_CHANNEL_1);
}
void stopPWMLeftD()
{
  HAL_TIM_PWM_Stop(&htim2,TIM_CHANNEL_1);
}
void stopPwmRightR()
{	
	HAL_TIM_PWM_Stop(&htim5,TIM_CHANNEL_1);
}
void stopPWMLeftR()
{
  HAL_TIM_PWM_Stop(&htim3,TIM_CHANNEL_3);
}
void changePwmRightD(uint16_t value)
{
	//HAL_TIM_PWM_Start(&htim4,TIM_CHANNEL_1);
	__HAL_TIM_SET_COMPARE(&htim4,TIM_CHANNEL_1,value);
}
void changePWMLeftD(uint16_t value)
{
	__HAL_TIM_SET_COMPARE(&htim2,TIM_CHANNEL_1,value);
  //HAL_TIM_PWM_Start(&htim2,TIM_CHANNEL_1);
}
void changePwmRightR(uint16_t value)
{
	__HAL_TIM_SET_COMPARE(&htim5,TIM_CHANNEL_1,value);
	//HAL_TIM_PWM_Start(&htim5,TIM_CHANNEL_1);
}
void changePWMLeftR(uint16_t value)
{
	__HAL_TIM_SET_COMPARE(&htim3,TIM_CHANNEL_3,value);
  //HAL_TIM_PWM_Start(&htim3,TIM_CHANNEL_3);
}
//**************ADD CHANGE TO MAX ON ANOTHER CHANNEL*************
void StartDrive(uint16_t pwm_value)
{
		changePwmRightD(pwm_value);
		changePWMLeftD(pwm_value);
		changePwmRightR(499);
		changePWMLeftR(499);
//	setPWMLeftR(499);
//	setPWMRightR(499);
//	setPWMRightD(pwm_value);
//	setPWMLeftD(pwm_value);
//	startPWMLeftR();
//	startPwmRightR();
//	startPWMLeftD();
//	startPwmRightD();	
}
void StartReverse(uint16_t pwm_value)
{
		changePwmRightD(499);
		changePWMLeftD(499);
		changePwmRightR(pwm_value);
		changePWMLeftR(pwm_value);
//	setPWMRightD(499);
//	setPWMLeftD(499);
//	setPWMLeftR(pwm_value);
//	setPWMRightR(pwm_value);
//	startPWMLeftR();
//	startPwmRightR();
//	startPWMLeftD();
//	startPwmRightD();
}
void StartLeft(uint16_t pwm_value)
{
		changePwmRightD(pwm_value);
		changePWMLeftD(499);
		changePwmRightR(499);
		changePWMLeftR(pwm_value);
//	setPWMLeftD(499);
//	setPWMRightR(499);
//	setPWMRightD(pwm_value);
//	setPWMLeftR(pwm_value);	
//	startPWMLeftR();
//	startPwmRightR();
//	startPWMLeftD();
//	startPwmRightD();
}
void StartRight(uint16_t pwm_value)
{
		changePwmRightD(499);
		changePWMLeftD(pwm_value);
		changePwmRightR(pwm_value);
		changePWMLeftR(499);
//	setPWMRightD(499);
//	setPWMLeftR(499);	
//	setPWMLeftD(pwm_value);
//	setPWMRightR(pwm_value);
//	startPWMLeftR();
//	startPwmRightR();
//	startPWMLeftD();
//	startPwmRightD();
}
void StopDrive()
{
	stopPwmRightD();
	stopPWMLeftD();
}
void StopReverse()
{
	stopPWMLeftR();
	stopPwmRightR();
}
void Stop()
{
	stopPWMLeftR();
	stopPwmRightR();
	stopPwmRightD();
	stopPWMLeftD();
}

void test(uint8_t* strRxBuf)
{
//	changePwmRightD(0);
//	changePWMLeftD(0);
//	changePwmRightR(0);
//	changePWMLeftR(0);
	//Stop();	
	switch (strRxBuf[0]) {
		case 0x01:
			//w - forward	
			StartDrive(500-strRxBuf[1]*10);//strRxBuf[1]*10-1
			break;
		case 0x02:
			//a - left
			StartLeft(500-strRxBuf[1]*10);
		  break;
		case 0x03:
			//s - back	
			StartReverse(500-strRxBuf[1]*10);		
			break;
		case 0x04:
			//d - right
			StartRight(500-strRxBuf[1]*10);
			break;	
		default:
			return;
			break;
	}
}
