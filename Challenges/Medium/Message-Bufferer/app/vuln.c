#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <signal.h>
#include <unistd.h>
#include <sys/types.h>

#define BUFSIZE 32
#define FLAGSIZE 64

char flag[FLAGSIZE];

void sigsegv_handler(int sig) {
    printf("\n%s\n", flag);
    fflush(stdout);
    exit(1);
}

void run_program(){

    FILE *f = fopen("flag.txt", "r");
    if (f == NULL) {
        printf("Please create 'flag.txt' in this directory with your own debugging flag.\n");
        exit(0);
    }

    fgets(flag, FLAGSIZE, f);
    fclose(f);

    signal(SIGSEGV, sigsegv_handler);

    printf("Please write your message here:\n");
    fflush(stdout);

    char message[BUFSIZE];
    scanf("%s", message);

    // Vulnerable point: printf without format string
    int count = printf(message);

    // Check if the length of the output is larger than twice the buffer size
    printf("\n");
    if (count > 2 * BUFSIZE) {
        printf("Your message length is larger than buffer size!\n");
        fflush(stdout);
    } else {
        printf("Thank you for your message!\n");
        fflush(stdout);
    }
}
void print_menu() {
    printf("\n1. Run the program"
           "\n2. Exit\n\nEnter your choice: ");
    fflush(stdout);
}

int main(int argc, char **argv) {
    int choice;

    while (1) {
        print_menu();
        int rval = scanf("%d", &choice);
        if (rval == EOF){
            exit(0);
        }
        if (rval != 1) {           
            exit(0);
        }

        switch (choice) {
        case 1:
            run_program();
            break;
        case 2:
            // exit
            return 0;
        default:
            printf("Invalid choice\n");
            fflush(stdout);
        }
    }
}
