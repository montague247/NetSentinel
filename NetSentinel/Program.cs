﻿using NetSentinel;

var handlers = ArgumentProcessor.Process(args);

if (handlers.Length == 0)
    ArgumentProcessor.GenerateHelp();
else
    ArgumentProcessor.Execute(handlers);
