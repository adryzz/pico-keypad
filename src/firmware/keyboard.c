#include <stdint.h>
#include "pico/stdlib.h"
#include "tusb_option.h"
#include "tusb.h"

typedef __uint8_t uint8_t;

/// Standard HID Boot Protocol Keyboard Report.
typedef struct KA_ATTR_PACKED
{
    uint8_t keycode[2]; /** Key codes of the currently pressed keys. */
} hid_keyboard_report;


//--------------------------------------------------------------------+
// Key class
//--------------------------------------------------------------------+

typedef struct 
{
    bool keyState;
    uint64_t debounce;
    uint64_t debounceTime;
    char keyCode;
    uint pin;
} key;

//--------------------------------------------------------------------+
// Early function déclaration
//--------------------------------------------------------------------+


int sense_key(key *k);
void send_report();

// --------------------------------------------------------------------+
//  Sending Keyboard Report
// --------------------------------------------------------------------+

static hid_keyboard_report report;
static uint8_t keycode[2] = { 0 };

static key key0 = {false, 0, 20000, 'z', 20};
static key key1 = {false, true, 0, 20000, 'x', 21};


void sense_task()
{
    // skip if hid is not ready yet
    if ( !tud_hid_ready() ) return;

    key0.keyState = sense_key(&key0);
    key1.keyState = sense_key(&key1);

    uint64_t time = time_us_64();

    // Send key 0
    if (key0.keyState)
    {
        if (keycode[0] == 0)
        {
            keycode[0] = key0.keyCode;
            send_report();
        }
        key0.debounce = time;
    }
    else
    {
        if (keycode[0] != 0)
        {
            if (time - key0.debounce > key0.debounceTime)
            {
                keycode[0] = 0;
                send_report();
            }
        }
    }

    // Send key 1
    if (key1.keyState)
    {
        if (keycode[1] == 0)
        {
            keycode[1] = key1.keyCode;
            send_report();
        }
        key1.debounce = time;
    }
    else
    {
        if (keycode[1] != 0)
        {
            if (time - key1.debounce > key1.debounceTime)
            {
                keycode[1] = 0;
                send_report();
            }
        }
    }
}

void send_report()
{
    memcpy(report.keycode, keycode, 2);  
    tud_hid_n_report(0, 1, &report, sizeof(report));
}

// --------------------------------------------------------------------+
//  GPIO pins tasks
// --------------------------------------------------------------------+

void prepare_GPIO()
{
    gpio_init(key0.pin);
    gpio_set_dir(key0.pin, false);
    gpio_pull_up(key0.pin);

    gpio_init(key1.pin);
    gpio_set_dir(key1.pin, false);
    gpio_pull_up(key1.pin);
}

bool sense_key(key *k)
{
    //it's a pullup, so 0 for set, nonzero for not set
    return gpio_get(*k.pin) == 0;
}

