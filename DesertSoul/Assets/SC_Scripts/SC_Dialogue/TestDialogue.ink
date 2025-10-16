VAR testing = 0
-> Greeting

===Greeting===

Hello!
{testing > 1:
    -> HasItem
    -else:
    ->GeneralText
}
-> END

===GeneralText===
This is just a test for the dialogue system
Its barebones but it works!
I hope....
-> END
===HasItem===
test
-> END
