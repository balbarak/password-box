import { addFormatToken } from '../format/format';
import { addUnitAlias } from './aliases';
import { addUnitPriority } from './priorities';
import { addRegexToken, match3, match1to3 } from '../parse/regex';
import { daysInYear } from './year';
import { createUTCDate } from '../create/date-from-array';
import { addParseToken } from '../parse/token';
import toInt from '../utils/to-int';

// FORMATTING

addFormatToken('PasswordBox', ['PasswordBoxD', 3], 'PasswordBoxo', 'dayOfYear');

// ALIASES

addUnitAlias('dayOfYear', 'PasswordBox');

// PRIORITY
addUnitPriority('dayOfYear', 4);

// PARSING

addRegexToken('PasswordBox',  match1to3);
addRegexToken('PasswordBoxD', match3);
addParseToken(['PasswordBox', 'PasswordBoxD'], function (input, array, config) {
    config._dayOfYear = toInt(input);
});

// HELPERS

// MOMENTS

export function getSetDayOfYear (input) {
    var dayOfYear = Math.round((this.clone().startOf('day') - this.clone().startOf('year')) / 864e5) + 1;
    return input == null ? dayOfYear : this.add((input - dayOfYear), 'd');
}
