INSERT INTO Projects (name, model) VALUES
   ('Моно усилитель НЧ 100 Вт', 'NM2033'),
   ('Устройство управления стоп-сигналами автомобиля', 'NM5403'),
   ('Генератор прямоугольных импульсов 250 Гц - 16 кГц', 'NS047'),
   ('Портативная радиостанция 27 МГц, AM', 'NM0704');

INSERT INTO Components (project_id, name, class, current, voltage) VALUES
   (1, 'R1', 'resistor', 0.1, 0.5),
   (2, 'C1','capacitor', 0.1, 0.5),
   (1, 'R2','resistor', 0.3, 0.7),
   (1, 'C2','capacitor', 0.1, 0.2);

INSERT INTO Classes (name) VALUES
   ('resistor'),
   ('voltage'),
   ('current'),
   ('capacitor');

INSERT INTO Tests (name) VALUES
   ('Слабая нагрузка'),
   ('Средняя нагрузка'),
   ('Сильная нагрузка');