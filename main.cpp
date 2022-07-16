#include <iostream>
#include <stdio.h> 
#include <stack>
#include <time.h>

#define AI 1
#define RANDOM 0

// add a comment to see how to use github in VS

// add another comment to see how to use github in VS

// the point player is allowed to remove from the total points
const int options[] = {8,7,3,1};

// Step Option structure for AI. 
// When an winning path is found( there's only 1 point left after AI's turn ),
// an entity would be push into the stack
struct Best_Path
{
    int steps_have_done;
    int points_left;
    int first_step;
};

// stack for storing winning path of AI
std::stack<Best_Path> mystack;

// random_bot pick away random points from allowed scope
int random_bot( int dots_left, int type );
// AI would list all possible point taking paths and pick the path that allow it to win
int Ai_bot( Best_Path i );

// Record the points taken away in the first step of the rest game process and points left after this turn
Best_Path init_path( int points_left, int first_step );


int main(){

    int n_points = 50;
    Best_Path AiLastPath;

    srand(time(0));

    // emulate the point taking process and stops when there is no point left and game came to the end
    // n_points is the initial points in the game
    do
    {
     // random play's turn
     n_points = random_bot(n_points, RANDOM);

    if (n_points <=0) break;

    // Ai's turn, when it comes to AI's turn, it needs to calculate the whole tree
    for( int first_step: options )
    {
        Ai_bot( init_path( n_points, first_step ));
    }

    // view possible best path, if there's best option, pick it, if no, pick a random option.
    while ( (!mystack.empty())  &&  (1 != mystack.top().points_left) )
    {
        mystack.pop();
    }
    
    if( mystack.empty() ) {
        printf( "Ai failed to find an ideal path , so " );
        n_points = random_bot( n_points, AI );
    }
    else{ 
        AiLastPath = mystack.top();
        n_points -= AiLastPath.first_step;
        printf( "Ai steps_left is %d, points_left is %d  pick is %d\n", AiLastPath.steps_have_done, n_points, AiLastPath.first_step ); 
    }
 
    // empty the stack for AI's next calculation
    mystack = std::stack<Best_Path>();

    } while (n_points > 0);
 
    return 0;    
}

/*
purpose: let player take away n points( n is in the scope of [1.3.7,8] ) randomly
*/
int random_bot( int dots_left, int type )
{
    const char* botType = (AI == type)? "AI":"Random";

    if (1 == dots_left ) {
        printf("Only one point left, %s bot lose!\n",botType);
        return -1;
    }
    

    if (dots_left <= 0) {
        printf("%s bot wins!\n", botType);
        return -1;
    }

    int i = 0;
    int k = 0;
    // loop until the option is smaller than dots_left
    do
    {
        k = rand()% 4;
        /* code */
    } while (options[k] > dots_left);
    
    i = options[k];

  //  while ( options[k]) > dots_left  );

    printf("%s randomly picks %d, Points left is %d\n", botType, i, dots_left - i);

    if( dots_left - i <=0 ) printf(" %s loses!", botType );

    return dots_left - i;
}


/*
enumerate all the possible path to the result of left point is 1. Pick the path that ensures when the left point is 1, it is the opponent's turn
*/
int Ai_bot( Best_Path k )
{
    if( k.points_left == 1 &&  1 == k.steps_have_done % 2 )
    {
        mystack.push(k);
        return 1;
    }
    
    if(k.points_left <= 0) 
    {
    return k.points_left;
    }

    for( int i: options )
    {
        return Ai_bot((Best_Path){k.steps_have_done + 1 , k.points_left - i, k.first_step}) ;
    }

    return -1;
}

Best_Path init_path( int points_left, int first_step )
{
    return (Best_Path){1, points_left - first_step , first_step};
}

