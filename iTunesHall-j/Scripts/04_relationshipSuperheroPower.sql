USE SuperheroesDb

CREATE TABLE SuperheroPowerRelation
(
SuperheroId INT NOT NULL ,
PowerId INT NOT NULL,
CONSTRAINT PK_SuperherPower PRIMARY KEY (SuperheroId, PowerId),
CONSTRAINT FK_SuperherPower_Superhero FOREIGN KEY (SuperheroId) REFERENCES Superhero(SuperheroId),
CONSTRAINT FK_SuperherPower_Power FOREIGN KEY (PowerId) REFERENCES Power(PowerId)
)