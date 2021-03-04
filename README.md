# ExtremeExpeditions
Skapar några grundläggande frågor mot databasen enligt CRUD. Create, read, Update and Delete. 

## Skapa databas 
Du kan använda din egna databas, eller så använder du samma som jag på föreläsningen. Läs i så fall in den backupfilen.
1. Skapa en databas i din lokala pgAdmin
2. Leta upp filen [MountainBackup.backup](https://github.com/systemvetenskap/ExtremeExpeditions/tree/master/ExtremeExpeditions/Databasbackup) i mappen databasbackup
3. Högerklicka på din nyskapade databas och välj restore
4. Sök upp filen du tankade ner i punkt 2 ovan och återställ.


För att du ska koppla upp dig mot databasen behöver du antingen byta till din användare (superuser) vid namn postgres och det lösenord du valde vid din installation, eller så gör du som det lite bättre alternativet att du lägger till en separat användare till ditt program. Mycket säkrare!

## Tilldela rättigheter
Först lägger du alltså till din användare i postgres. Därefter måste du tilldela rättigheter: GRANT

Här ger vi användaren rättigheter att göra allt på alla tabeller i din databas:
```sql 
GRANT ALL ON ALL TABLES IN SCHEMA public TO demouser
```

Dock måste du dessutom ge den tillgång till dina sekvenser = de metoder som krävs för att automatiskt skapa nya id-nummer på primärnycklarna
```sql
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO demouser
```
