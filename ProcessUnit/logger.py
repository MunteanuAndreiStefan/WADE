import functools
import logging

logger = logging.getLogger('ProcessUnit')
logger.setLevel(logging.DEBUG)

ch = logging.FileHandler('process_unit_log.txt')
ch.setLevel(logging.DEBUG)

formatter = logging.Formatter('[%(asctime)s] %(message)s')
ch.setFormatter(formatter)
logger.addHandler(ch)


def log(message):
    logging.getLogger('ProcessUnit').debug("{0}MSG   {1}".format('\t' * LogDecorator.indent, message))


class LogDecorator(object):
    indent = 0

    def __init__(self):
        self.logger = logging.getLogger('ProcessUnit')

    def __call__(self, fn):
        @functools.wraps(fn)
        def decorated(*args, **kwargs):
            try:
                self.logger.debug(
                    "{0}START {1} - {2} - {3}".format('\t' * LogDecorator.indent, fn.__name__, args, kwargs))
                LogDecorator.indent += 1
                result = fn(*args, **kwargs)
                LogDecorator.indent -= 1
                self.logger.debug("{0}END   {1} - {2}".format('\t' * LogDecorator.indent, fn.__name__, result))
                return result
            except Exception as ex:
                self.logger.debug("Exception {0}".format(ex))
                raise ex
            return result

        return decorated
