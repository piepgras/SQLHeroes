USE SuperheroesDb

ALTER TABLE Assistant
ADD SuperheroId INT 
CONSTRAINT FK_SuperheroAssistant FOREIGN KEY (SuperheroId) REFERENCES Superhero(SuperheroId);