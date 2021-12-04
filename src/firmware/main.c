#include <stdio.h>
#include "pico/stdlib.h"
#include <string.h>

#include "bsp/board.h"
#include "tusb.h"
#include "keyboard.h"

int main(void)
{
    //TODO: read config

    board_init(); // Initialize stuff such as led?
    tusb_init(); // initialize the device

    board_led_write(true);

    prepare_GPIO();

    //do stuff
    while(1)
    {
        tud_task(); // tinyusb device task, handle event such as mount and unmout, used to provide the descriptor and configs
        sense_task();
    }

    return 0;
}
