﻿  CREATE TABLE Comment (
                  id INTEGER UNSIGNED DEFAULT NULL AUTO_INCREMENT,
                  mainid INTEGER UNSIGNED NOT NULL,
                  nodeid INTEGER UNSIGNED NOT NULL,
                  name VARCHAR(250),
                  email VARCHAR(250),
                  website VARCHAR(250),
                  comment TEXT,
                  spam BOOLEAN,
                  ham BOOLEAN,
                  created DATETIME,
                  PRIMARY KEY (id)
                  );