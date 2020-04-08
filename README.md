# TransactionsCalculator
A simple custom transactions calculator for a friend of mine.

The software is still a console application, but in future it will become a desktop application with a GUI.

## Basic functionality
The software takes as an input a directory path and elaborates all the files that have a *.txt* extention. 

The files that are processed by the application are TAB separated files with strictly defined column headers and types.

For each file, a number of calculations is performed. In practice, each calculation is like a query, which first applies a filter on the file rows and then applies some mathematical operations on certain columns.

 The results of the calculations, which are performed over all the *.txt* files in the specified directory, are presented as a report, that is in turn persisted on the local hard drive. 

Actually, 2 reports containing the same information are produced: one in PDF format and one in EXCEL format.

Please refer to the wiki for more business rules details.


