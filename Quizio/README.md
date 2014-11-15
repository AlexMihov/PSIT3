C# Project
=====

In this Folder you can find the C# relevant parts of the Application.

This Folder contains the business logic with the view!



### JSON Structure

## Get Categories

Sample JSON for category list:

```javascript
[
  {
    id: 1,
    name: 'Englisch',
    description: 'Alle Quizzes die mit Englisch zu tun haben.',
    quizies: 
    [
      {
        id: 1,
        title: 'Thesaurus',
        description: 'Something about Thesaurus.'
      },
      {
        id: 2,
        title: 'Vokabulary',
        description: 'Something about Vokabulary.'
      },
      {
        id: 3,
        title: 'Grammar',
        description: 'Something about Grammar.'
      },
      {
        id: 4,
        title: 'True or false',
        description: 'Something about True or false.'
      }
    ]
  },
  {
    id: 2,
    name: 'Mathematik',
    description: 'Lerne besser zu rechnen.',
    quizies: 
    [
      {
        id: 1,
        title: 'Gleichungen',
        description: 'Etwas über Gleichungen.'
      },
      {
        id: 2,
        title: 'Satzaufgaben',
        description: 'Etwas über Satzaufgaben.'
      },
      {
        id: 3,
        title: 'Prosa',
        description: 'Etwas über Prosa.'
      },
      {
        id: 4,
        title: 'Relationale Algebra',
        description: 'Etwas über Relationale Algebra.'
      }
    ]
  }
];
```
